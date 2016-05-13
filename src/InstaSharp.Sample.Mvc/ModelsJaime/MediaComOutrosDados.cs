using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaSharp.Sample.Mvc.ModelsJaime
{

    /// <summary>
    /// Temporary just to test another endpoint
    /// </summary>
    public class MediaComOutrosDados
    {
        public InstaSharp.Models.Media Media { get; set; }

        public int TotalLikesMedia { get; set; } // We can use InstaSharp.Models.Media.Likes
    }
}