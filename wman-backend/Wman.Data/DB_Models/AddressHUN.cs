using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wman.Data.DB_Models
{
    public class AddressHUN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZIPCode { get; set; }
        public string BuildingNumber { get; set; }
        public string Floordoor { get; set; }
        [JsonIgnore]
        public virtual ICollection<WorkEvent> WorkEvents { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;
            return CompareAddresses(this, obj as AddressHUN);
        }

        private bool CompareAddresses(AddressHUN address1, AddressHUN address2)
        {
            return address1.City == address2.City && address1.Street == address2.Street && address1.ZIPCode == address2.ZIPCode;
        }
        public AddressHUN()
        {
            this.WorkEvents = new List<WorkEvent>();
        }
        public override string ToString()
        {
            string output = ZIPCode + " " +
                   City + ", " +
                   Street + " " +
                   BuildingNumber;
            if (!String.IsNullOrWhiteSpace(Floordoor))
            {
                output += " (" + Floordoor + ")";
            }
            return output;
        }
    }
}
