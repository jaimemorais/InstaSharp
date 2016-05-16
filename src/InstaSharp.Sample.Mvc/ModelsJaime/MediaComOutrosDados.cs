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

        public int TotalLikesMedia { get; set; } // just for testing. we can use InstaSharp.Models.Media.Likes

        public List<string> UsuariosQueDeramLike { get; set; }
    }
}