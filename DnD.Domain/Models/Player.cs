using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class Player
    {

        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime? DateJoined { get; set; }
        public ICollection<Character>? Characters { get; set; }


    }
}
