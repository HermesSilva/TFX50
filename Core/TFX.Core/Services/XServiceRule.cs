using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using TFX.Core.Controllers;

using TFX.Core.Model;

namespace TFX.Core.Services
{
    public interface XIBaseServiceRule
    {
    }

    public interface XIJobServiceRule : XIBaseServiceRule
    {
        void Execute()
        {
        }

        void InternalExecute()
        {
        }
    }

    public interface XIServiceRule<T, P> : XIBaseServiceRule where T : XDataTuple where P : XDataTuple
    {
        void InternalAfterFlush(List<P> pTuples);
        List<T> InternalAfterSelect(List<T> pTuples);
        List<P> InternalBeforeFlush(List<P> pTuples);
    }
    public abstract class XControllerINFRule<T> where T : XController
    {
        public XControllerINFRule(T pController)
        {
            Controller = pController;
        }

        protected T Controller
        {
            get;
        }
    }

    public abstract class XServiceINFRule<S, T> where S : XService where T : XDataTuple
    {
        public XServiceINFRule(S pService)
        {
            Service = pService;
        }

        protected S Service
        {
            get;
        }
        public void InternalBeforeExecute()
        {
            BeforeExecute();
        }

        protected virtual void BeforeExecute()
        {
        }

        public void InternalAfterExecute(List<T> pTuples)
        {
            AfterExecute(pTuples);
        }
        protected virtual void AfterExecute(List<T> pTuples)
        {
        }
    }

    public abstract class XServiceRule<T, P> : XIServiceRule<T, P> where T : XDataTuple where P : XDataTuple
    {
        public XServiceRule(XService pService)
        {
            Service = pService;
        }
        protected XService Service
        {
            get;
        }

        List<T> XIServiceRule<T, P>.InternalAfterSelect(List<T> pTuples)
        {
            return AfterSelect(pTuples);
        }

        protected virtual List<T> AfterSelect(List<T> pTuples)
        {
            return pTuples;
        }

        public void InternalAfterFlush(List<P> pTuples)
        {
        }

        public List<P> InternalBeforeFlush(List<P> pTuples)
        {
            return BeforeFlush(pTuples);
        }

        protected virtual List<P> BeforeFlush(List<P> pTuples)
        {
            return pTuples;
        }
    }
}
