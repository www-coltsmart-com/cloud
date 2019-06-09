using ColtSmart.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColtSmart.Entity
{
    public class DeviceNet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string DeviceId { get; set; }

        public double NetFlow { get; set; }
    }
}
