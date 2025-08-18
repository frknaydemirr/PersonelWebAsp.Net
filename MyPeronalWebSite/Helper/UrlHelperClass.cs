using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MyPeronalWebSite.Helper
{

    // SEO uyumlu URL için kullanılır:
    public static class UrlHelperClass
    {
        public static string FriendlyURLTitle(this UrlHelper urlHelper, string incomingText)
        {
            if (string.IsNullOrEmpty(incomingText))
                return incomingText;

            // URL'de kullanılamayan karakterleri temizle (Unicode karakterleri koru)
            var friendlyText = Regex.Replace(incomingText, @"[^\p{L}\p{N}\s-]", "");

            // Boşlukları tireye dönüştür
            friendlyText = friendlyText.Replace(" ", "-");

            // Tüm harfleri küçült
            friendlyText = friendlyText.ToLower();

            // Birden fazla tireyi tek tireye indirge
            friendlyText = Regex.Replace(friendlyText, @"-+", "-");

            // Başta ve sondaki tireleri kaldır
            friendlyText = friendlyText.Trim('-');

            return friendlyText;
        }
    }
}





