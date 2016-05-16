﻿using InstaSharp.Endpoints;
using InstaSharp.Models;
using InstaSharp.Models.Responses;
using InstaSharp.Sample.Mvc.ModelsJaime;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InstaSharp.Sample.Mvc.Controllers
{
    public class HomeController : Controller
    {
        static string clientId = ConfigurationManager.AppSettings["instagram_client_id"];
        static string clientSecret = ConfigurationManager.AppSettings["instagram_client_secret"];
        static string redirectUri = ConfigurationManager.AppSettings["redirectUri"];

        InstagramConfig config = new InstagramConfig(clientId, clientSecret, redirectUri, "");

        // GET: Home
        public ActionResult Index()
        {
            var oAuthResponse = Session["InstaSharp.AuthInfo"] as OAuthResponse;

            if (oAuthResponse == null)
            {
                return RedirectToAction("Login");
            }

            UserInfo userInfo = oAuthResponse.User;

            return View(userInfo);            
        }

        public ActionResult Login()
        {
            var scopes = new List<OAuth.Scope>();
            scopes.Add(InstaSharp.OAuth.Scope.Basic);
            scopes.Add(InstaSharp.OAuth.Scope.Public_Content);
            scopes.Add(InstaSharp.OAuth.Scope.Follower_List);
            scopes.Add(InstaSharp.OAuth.Scope.Comments);
            scopes.Add(InstaSharp.OAuth.Scope.Relationships);
            scopes.Add(InstaSharp.OAuth.Scope.Likes);

            var link = InstaSharp.OAuth.AuthLink(config.OAuthUri + "authorize", config.ClientId, config.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);

            return Redirect(link);
        }


        public async Task<ActionResult> MyFeed()
        {
            var oAuthResponse = Session["InstaSharp.AuthInfo"] as OAuthResponse;

            if (oAuthResponse == null)
            {
                return RedirectToAction("Login");
            }



            Users usersEndpoint = new Endpoints.Users(config, oAuthResponse);
            

            MediasResponse mediasResponse = await usersEndpoint.RecentSelf();
            List<InstaSharp.Models.Media> listaMedias = mediasResponse.Data;
            

            // testing likes endpoint
            List<MediaComOutrosDados> listaMediasComOutrosDados = new List<MediaComOutrosDados>();
            InstaSharp.Endpoints.Likes likesEndpoint = new Endpoints.Likes(config, oAuthResponse);

            foreach (InstaSharp.Models.Media media in listaMedias)
            {

                // ** MODO SANDBOX : 
                // Data is restricted to the 10 users and the 20 most recent media from each of those users

                MediaComOutrosDados mc = new MediaComOutrosDados();
                mc.Media = media;

                // total likes
                UsersResponse urLike = await likesEndpoint.Get(media.Id);
                int totalLikesMedia = urLike.Data.Count;
                mc.TotalLikesMedia = totalLikesMedia;    //or mc.TotalLikesMedia = media.Likes.Count;
                
                // usuarios que deram like
                List<User> usuariosQueDeramLike = urLike.Data;
                List<string> usernamesQueDeramLike = new List<string>();
                if (usuariosQueDeramLike != null)
                {
                    foreach (User userLike in usuariosQueDeramLike)
                    {
                        usernamesQueDeramLike.Add(userLike.Username);
                    }
                }
                mc.UsuariosQueDeramLike = usernamesQueDeramLike;


                listaMediasComOutrosDados.Add(mc);
            }


            return View(listaMediasComOutrosDados);
        }
                
        public async Task<ActionResult> OAuth(string code)
        {
            // add this code to the auth object
            var auth = new OAuth(config);

            // now we have to call back to instagram and include the code they gave us
            // along with our client secret
            var oauthResponse = await auth.RequestToken(code);

            // both the client secret and the token are considered sensitive data, so we won't be
            // sending them back to the browser. we'll only store them temporarily.  If a user's session times
            // out, they will have to click on the authenticate button again - sorry bout yer luck.
            Session.Add("InstaSharp.AuthInfo", oauthResponse);

            // all done, lets redirect to the home controller which will send some intial data to the app
            return RedirectToAction("Index");
        }
    }

}