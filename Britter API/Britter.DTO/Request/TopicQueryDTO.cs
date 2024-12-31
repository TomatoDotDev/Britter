using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Request
{
    /// <summary>
    /// Topic query data transfer object.
    /// </summary>
    public class TopicQueryDTO
    {
        /// <summary>
        /// Topic ID.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The topic title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The topic description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
