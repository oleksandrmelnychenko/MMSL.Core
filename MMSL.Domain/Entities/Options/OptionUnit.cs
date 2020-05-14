﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MMSL.Domain.Entities.Options {
    public class OptionUnit : EntityBase {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ImageUrl { get; set; }

        public long OptionGroupId { get; set; }

        public OptionGroup OptionGroup { get; set; }
    }
}
