using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace HP.Helpers
{
    public static class DisplayHelper
    {
        public static IHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> exp)
        {
            var metadata = ModelMetadata.FromLambdaExpression(exp, helper.ViewData);
            var content = metadata.ShortDisplayName ?? metadata.DisplayName ?? string.Empty;
            return new HtmlString(content);           
        }
        public static IHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> exp)
        {
            var metadata = ModelMetadata.FromLambdaExpression(exp, helper.ViewData);
            var content = metadata.Description ?? string.Empty;
            return new HtmlString(content);
        }
    }
}