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
            Task.Run(() => {
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
            Task.Run(() => {
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
            DrawFabricHeader(doc, fabric);
            DrawFabricInfo(doc, fabric);

            try
            {
                string serverImagePath = Path.Combine(ConfigurationManager.UploadsPath, fabric.ImageUrl);
            }
            catch (Exception exc)
            {

            }


        }

        private void DrawFabricHeader(Document doc, Fabric fabric)
        {

            Paragraph heading = new Paragraph(fabric.FabricCode, new Font(Font.HELVETICA, 22f, Font.BOLD));
            heading.SpacingAfter = .3f;

            Paragraph description = null;

            if (!string.IsNullOrEmpty(fabric.Description))
            {
                description = new Paragraph(fabric.Description, new Font(Font.HELVETICA, 16f, Font.NORMAL, BaseColor.Gray));
                description.SpacingAfter = 18f;
            }

            doc.Add(heading);

            if (description != null)
            {
                doc.Add(description);
            }
        }

        private void DrawFabricInfo(Document doc, Fabric fabric)
        {
            void draw(string title, string value)
            {
                Chunk meters = new Chunk(title, new Font(Font.HELVETICA, 16f, Font.NORMAL, BaseColor.Gray));
                Chunk metersValue = new Chunk(value, new Font(Font.HELVETICA, 16f, Font.NORMAL, BaseColor.Black));

                Phrase metersPhrase = new Phrase();
                metersPhrase.Add(meters);
                metersPhrase.Add(metersValue);

                Paragraph paragraph = new Paragraph();
                //paragraph.Alignment = Element.ALIGN_JUSTIFIED;
                paragraph.Add(metersPhrase);
                paragraph.SpacingAfter = .3f;

                doc.Add(paragraph);
            }

            draw("Meters:", $"{fabric.Metres}");
            draw("Mill:", $"{fabric.Mill}");
            draw("Color:", $"{fabric.Color}");
            draw("Composition:", $"{fabric.Composition}");
            draw("GSM:", $"{fabric.GSM}");
            draw("Count:", $"{fabric.Count}");
            draw("Weave:", $"{fabric.Weave}");
            draw("Pattern:", $"{fabric.Pattern}");
        }
    }
}
