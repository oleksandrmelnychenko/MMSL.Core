using iTextSharp.text;
using iTextSharp.text.pdf;
using MMSL.Common;
using MMSL.Domain.DataContracts.Fabrics;
using MMSL.Domain.DataContracts.Filters;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Entities.Fabrics;
using MMSL.Domain.EntityHelpers;
using MMSL.Domain.Repositories.Fabrics.Contracts;
using MMSL.Services.FabricServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MMSL.Services.FabricServices
{
    public class FabricService : IFabricService
    {

        private readonly IFabricRepositoriesFactory _fabricRepositoriesFactory;

        private readonly IDbConnectionFactory _connectionFactory;

        public FabricService(IFabricRepositoriesFactory fabricRepositoriesFactory, IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _fabricRepositoriesFactory = fabricRepositoriesFactory;
        }

        public Task<Fabric> GetByIdAsync(long fabricId) =>
             Task.Run(() =>
             {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                 {
                     return _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetById(fabricId);
                 }
             });

        public Task<Fabric> AddFabric(NewFabricDataContract fabric, long userIdentityId, string imageUrl = null) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = fabric.GetEntity();
                    fabricEntity.UserIdentityId = userIdentityId;

                    if (!string.IsNullOrEmpty(imageUrl))
                        fabricEntity.ImageUrl = imageUrl;

                    FabricVisibilitiesDataContract visibilities = repository.GetFabricsVisibilities(userIdentityId);

                    if (visibilities != null)
                    {
                        fabricEntity.IsColorVisible = visibilities.IsColorVisible;
                        fabricEntity.IsCompositionVisible = visibilities.IsCompositionVisible;
                        fabricEntity.IsCountVisible = visibilities.IsCountVisible;
                        fabricEntity.IsGSMVisible = visibilities.IsGSMVisible;
                        fabricEntity.IsMetresVisible = visibilities.IsMetresVisible;
                        fabricEntity.IsMillVisible = visibilities.IsMillVisible;
                        fabricEntity.IsPatternVisible = visibilities.IsPatternVisible;
                        fabricEntity.IsWeaveVisible = visibilities.IsWeaveVisible;
                    }

                    return repository.AddFabric(fabricEntity);
                }
            });

        public Task<Fabric> UpdateFabric(UpdateFabricDataContract fabric, string imageUrl = null) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = fabric.GetEntity();

                    if (!string.IsNullOrEmpty(imageUrl))
                        fabricEntity.ImageUrl = imageUrl;

                    return repository.UpdateFabric(fabricEntity);
                }
            });

        public Task<Fabric> DeleteFabric(long fabricId) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    Fabric fabricEntity = repository.GetById(fabricId);

                    if (fabricEntity == null)
                        throw new Exception("Fabric not found");

                    fabricEntity.IsDeleted = true;

                    return repository.UpdateFabric(fabricEntity);
                }
            });

        public Task UpdateFabricVisibilities(FabricVisibilitiesDataContract fabric, long userIdentityId) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    //Fabric defaultFabricEntity = repository.GetById(fabric.Id);

                    //defaultFabricEntity = fabric.MapFabric(defaultFabricEntity);

                    repository.UpdateFabricVisibilities(fabric, userIdentityId);

                    //return repository.GetById(defaultFabricEntity.Id);
                }
            });

        public Task<FabricVisibilitiesDataContract> GetFabricVisibilities(long userIdentityId) =>
           Task.Run(() =>
           {
               using (IDbConnection connection = _connectionFactory.NewSqlConnection())
               {
                   IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                   return repository.GetFabricsVisibilities(userIdentityId);
               }
           });

        public Task<PaginatingResult<Fabric>> GetFabrics(int pageNumber, int limit, string searchPhrase, FilterItem[] filters, long? ownerUserIdentityId) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetPagination(pageNumber, limit, searchPhrase, filters, ownerUserIdentityId);
                }
            });

        public Task<List<FilterItem>> GetFabricFilters(long? ownerUserIdenetity) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetFilters(ownerUserIdenetity);
                }
            });

        public Task<string> PrepareFabricsPdf(string searchPhrase, FilterItem[] filters, long? ownerUserIdentityId) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IEnumerable<Fabric> fabrics = _fabricRepositoriesFactory
                        .NewFabricRepository(connection)
                        .GetAllFabrics(searchPhrase, filters, ownerUserIdentityId);

                    string fullFilePath = Path.Combine(ConfigurationManager.UploadsPath, $"Fabrics_{DateTime.Now.Ticks}.pdf");

                    //TODO: create pdf
                    Document doc1 = new Document(PageSize.A4);

                    //foreach (Fabric fabric in fabrics)
                    //{
                    //    //TODO: check if path correct
                    //    string serverImagePath = Path.Combine(ConfigurationManager.UploadsPath, fabric.ImageUrl);
                    //}

                    using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
                    {
                        PdfWriter.GetInstance(doc1, fs);
                        DrawPDF(doc1, fabrics);
                    }

                    return fullFilePath;

                }
            });

        public Task<IEnumerable<Fabric>> AddFabrics(IEnumerable<Fabric> fabrics, long identityId) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IFabricRepository repository = _fabricRepositoriesFactory.NewFabricRepository(connection);

                    foreach (Fabric fabric in fabrics)
                    {
                        fabric.UserIdentityId = identityId;

                        Fabric created = repository.AddFabric(fabric);
                        fabric.Id = created.Id;
                    }

                    return fabrics;
                }
            });

        private void DrawPDF(Document doc, IEnumerable<Fabric> fabrics)
        {
            try
            {
                doc.Open();

                foreach (Fabric fabric in fabrics)
                {
                    DrawFabric(doc, fabric);
                }

                doc.Close();
            }
            catch (Exception exc)
            {
                Debugger.Break();
            }
        }

        private void DrawFabric(Document doc, Fabric fabric)
        {


            Foo(doc, fabric);
        }

        private void Foo(Document doc, Fabric fabric)
        {

            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 500f;
            table.LockedWidth = true;

            PdfPCell header = new PdfPCell(new Phrase(fabric.FabricCode, new Font(Font.HELVETICA, 14f, Font.BOLD, BaseColor.Black)));
            header.Colspan = 2;
            header.BorderWidth = 0;
            table.AddCell(header);

            PdfPCell description = new PdfPCell(new Phrase(fabric.Description));
            description.Colspan = 2;
            description.PaddingTop = 6f;
            description.BorderWidth = 0;
            table.AddCell(description);

            PdfPTable nested = new PdfPTable(1);
            if (fabric.IsMetresVisible)
                nested.AddCell(buildInfo("Metters", $"{fabric.Metres}"));

            if (fabric.IsMillVisible)
                nested.AddCell(buildInfo("Mill", $"{fabric.Mill}"));

            if (fabric.IsColorVisible)
                nested.AddCell(buildInfo("Color", $"{fabric.Color}"));

            if (fabric.IsCompositionVisible)
                nested.AddCell(buildInfo("Composition", $"{fabric.Composition}"));

            if (fabric.IsGSMVisible)
                nested.AddCell(buildInfo("GSM", $"{fabric.GSM}"));

            if (fabric.IsCountVisible)
                nested.AddCell(buildInfo("Count", $"{fabric.Count}"));

            if (fabric.IsWeaveVisible)
                nested.AddCell(buildInfo("Weave", $"{fabric.Weave}"));

            if (fabric.IsPatternVisible)
                nested.AddCell(buildInfo("Pattern", $"{fabric.Pattern}"));

            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.PaddingTop = 12f;
            nesthousing.BorderWidth = 0;
            table.AddCell(nesthousing);


            PdfPCell imageCell = buildImageCell(fabric);
            table.AddCell(imageCell);


            PdfPCell bottomLine = new PdfPCell();
            bottomLine.Colspan = 2;
            bottomLine.BorderWidth = 0;
            bottomLine.BorderWidthBottom = .5f;
            bottomLine.PaddingTop = 18f;
            table.AddCell(bottomLine);

            table.SpacingAfter = 18f;
            doc.Add(table);
        }

        PdfPCell buildInfo(string title, string info)
        {
            Phrase phrase = new Phrase();
            Chunk titleChunk = new Chunk($"{title}: ", new Font(Font.HELVETICA, 12f, Font.NORMAL, BaseColor.Gray));
            Chunk infoChunk = new Chunk($" {info}", new Font(Font.HELVETICA, 12f, Font.NORMAL, BaseColor.Black));
            phrase.Add(titleChunk);
            phrase.Add(infoChunk);

            PdfPCell result = new PdfPCell(phrase);
            result.PaddingTop = 4f;
            result.PaddingRight = 6f;
            result.BorderWidth = 0;

            return result;
        }

        PdfPCell buildImageCell(Fabric fabric)
        {
            PdfPCell result = null;

            try
            {
                string serverImagePath = Path.Combine(ConfigurationManager.UploadsPath, Path.GetFileName(fabric.ImageUrl));

                Image jpg = Image.GetInstance(serverImagePath);
                jpg.ScaleToFit(250f, 250f);

                result = new PdfPCell(jpg);
                result.BorderWidth = 0;
                result.PaddingTop = 16f;
                result.Colspan = 2;
            }
            catch (Exception exc)
            {
                result = new PdfPCell();
                result.BorderWidth = 0;
                result.PaddingTop = 16f;
                result.Colspan = 2;
            }

            return result;
        }
    }
}
