using System;
using WasteManagement.Models.Enums;

namespace WasteManagement.Models.Models
{

    public class WasteContainerDtoModel : RepositoryEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal FullPecentage { get; set; }
        public ContainerSizeEnum Size { get; set; }
    }

    public class WasteContainerCreateModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal FullPecentage { get; set; }
        public ContainerSizeEnum Size { get; set; }
    }

    public class WasteContainerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal FullPecentage { get; set; }
        public ContainerSizeEnum Size { get; set; }
    }

    public class WasteContainerUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal FullPecentage { get; set; }
        public ContainerSizeEnum Size { get; set; }
    }

    public class WasteContainerDeleteModel
    {
        public Guid Id { get; set; }
    }
}
