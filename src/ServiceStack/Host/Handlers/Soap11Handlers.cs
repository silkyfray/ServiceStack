﻿#if !NETSTANDARD1_6

using ServiceStack.Metadata;
using ServiceStack.Web;

namespace ServiceStack.Host.Handlers
{
    public class Soap11Handler : SoapHandler
    {
        public Soap11Handler(RequestAttributes soapType) : base(soapType) { }

        protected override System.ServiceModel.Channels.Message GetRequestMessageFromStream(System.IO.Stream requestStream)
        {
            return GetSoap11RequestMessage(requestStream);
        }
    }

    public class Soap11ReplyHandler : Soap11Handler
    {
        public Soap11ReplyHandler() : base(RequestAttributes.Soap11) { }
    }

    public class Soap11OneWayHandler : Soap11Handler
    {
        public Soap11OneWayHandler() : base(RequestAttributes.Soap11) { }
    }

    public class Soap11MessageReplyHttpHandler : Soap11Handler
    {
        public Soap11MessageReplyHttpHandler() : base(RequestAttributes.Soap11) { }

        public override void ProcessRequest(IRequest httpReq, IResponse httpRes, string operationName)
        {
            if (httpReq.Verb == HttpMethods.Get)
            {
                var wsdl = new Soap11WsdlMetadataHandler();
                wsdl.Execute(httpReq, httpRes);
                return;
            }

            var responseMessage = Send(null, httpReq, httpRes);

            if (httpRes.IsClosed)
                return;

            HostContext.AppHost.WriteSoapMessage(httpReq, responseMessage, httpRes.OutputStream);
        }
    }
}

#endif
