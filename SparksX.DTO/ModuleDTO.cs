using System.Collections.Generic;

namespace SparksX.DTO
{
    public class ModuleDTO
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }

        public List<ModuleInteractionDTO> ModuleInteractionList { get; set; }
    }
}
