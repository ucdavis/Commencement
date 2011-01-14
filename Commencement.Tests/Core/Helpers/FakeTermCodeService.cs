using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Commencement.Tests.Core.Helpers
{
    public static class FakeTermCodeService
    {
        #region Helpers

        public static void LoadTermCodes(string termCode, IRepository<TermCode> termCodeRepository, bool nonActive = false)
        {
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = !nonActive;
            ControllerRecordFakes.FakeTermCode(0, termCodeRepository, termCodes);
            termCodes[0].SetIdTo(termCode);

            var context = CreateHttpContext("index.aspx", "http://test.org/index.aspx", null);
            var result = RunInstanceMethod(Thread.CurrentThread, "GetIllogicalCallContext", new object[] { });
            SetPrivateInstanceFieldValue(result, "m_HostContext", context);
            if (nonActive)
            {
                HttpContext.Current.Cache["NoSuchBird"] = string.Empty;
                if (HttpContext.Current.Cache.Count > 1)
                {
                    try
                    {
                        HttpContext.Current.Cache.Remove("CurrentTerm");
                    }
                    catch (Exception)
                    {
                       
                    }
                }
            }
            else
            {
                HttpContext.Current.Cache["CurrentTerm"] = termCodeRepository.Queryable.First();
            }
        }


        private static HttpContext CreateHttpContext(string fileName, string url, string queryString)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var hres = new HttpResponse(sw);
            var hreq = new HttpRequest(fileName, url, queryString);
            var httpc = new HttpContext(hreq, hres);
            return httpc;
        }

        private static object RunInstanceMethod(object source, string method, object[] objParams)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var type = source.GetType();
            var m = type.GetMethod(method, flags);
            if (m == null)
            {
                throw new ArgumentException(string.Format("There is no method '{0}' for type '{1}'.", method, type));
            }

            var objRet = m.Invoke(source, objParams);
            return objRet;
        }

        public static void SetPrivateInstanceFieldValue(object source, string memberName, object value)
        {
            var field = source.GetType().GetField(memberName, BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                throw new ArgumentException(string.Format("Could not find the private instance field '{0}'", memberName));
            }

            field.SetValue(source, value);
        }
        #endregion Helpers
    }
}
