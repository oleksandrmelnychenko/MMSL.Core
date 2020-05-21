﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Options {
    public class OptionUnit : EntityBase {

        public int OrderIndex { get; set; }

        public string Value { get; set; }

        public string ImageUrl { get; set; }

        public string ImageName { get; set; }

        public bool IsMandatory { get; set; }

        public long OptionGroupId { get; set; }

        public OptionGroup OptionGroup { get; set; }
    }
}
