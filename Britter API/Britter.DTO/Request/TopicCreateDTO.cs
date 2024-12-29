using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Request
{
    /// <summary>
    /// Topic creation DTO.
    /// </summary>
    public class TopicCreateDTO
    {
        /// <summary>
        /// Topic ID.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Topic Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Topic Description.
        /// </summary>
        public string Description { get; set; }
    }
}
