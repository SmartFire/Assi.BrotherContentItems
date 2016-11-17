using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Assi.BrotherContentItems.Models;

namespace Assi.BrotherContentItems {
    public class Migrations : DataMigrationImpl {

        public int Create() {
			ContentDefinitionManager.AlterPartDefinition(typeof(BrothersPart).Name, cfg =>
                 cfg.Attachable());
             return 1;
        }
    }
}