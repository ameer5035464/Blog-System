using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL.DtoModels.AccountDtos
{
    public class ResultDto
    {
        public string? Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}
