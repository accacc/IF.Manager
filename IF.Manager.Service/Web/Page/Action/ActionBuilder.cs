using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Web.Page.Action
{
    public class ActionBuilder
    {
        private Dictionary<string,string> HtmlAttributes { get; set; }

        public ActionBuilder(string Text,string Url)
        {
            this.HtmlAttributes = new Dictionary<string, string>();

        }


        public ActionBuilder UpdateId(string updateId)
        {
            this.HtmlAttributes.Add("if-ajax-update-id", updateId);
            return this;
        }

        public ActionBuilder modalid(string modalid)
        {
            this.HtmlAttributes.Add("if-ajax-modal-id", modalid);
            return this;
        }

        public ActionBuilder refreshGrid(string refreshGrid)
        {
            this.HtmlAttributes.Add("if-ajax-refresh-grid", refreshGrid);
            return this;
        }

        public ActionBuilder showDialog(string showDialog)
        {
            this.HtmlAttributes.Add("if-ajax-show-dialog", showDialog);
            return this;
        }

        public ActionBuilder insertionMode(string insertionMode)
        {
            this.HtmlAttributes.Add("if-ajax-insertion-mode", insertionMode);
            return this;
        }


        public ActionBuilder method(string method)
        {

            this.HtmlAttributes.Add("if-ajax-method", method);
            return this;
        }

        public ActionBuilder enctype(string enctype)
        {
            this.HtmlAttributes.Add("if-ajax-enctype", enctype);
            return this;
        }

        public ActionBuilder CloseModalAfterSuccess(string CloseModalAfterSuccess)
        {
            this.HtmlAttributes.Add("if-ajax-close-modal-on-success", CloseModalAfterSuccess);
            return this;
        }


        public ActionBuilder gridViewId(string gridViewId)
        {
            this.HtmlAttributes.Add("if-ajax-gridview-id", gridViewId);
            return this;
        }

        public ActionBuilder onErrorFunc(string onErrorFunc)
        {
            this.HtmlAttributes.Add("if-ajax-onerror-func", onErrorFunc);
            return this;
        }


        public ActionBuilder onSuccessFunc(string onSuccessFunc)
        {
            this.HtmlAttributes.Add("if-ajax-onsuccess-func", onSuccessFunc);
            return this;
        }



        public ActionBuilder onSuccessReload(string onSuccessReload)
        {
            this.HtmlAttributes.Add("if-ajax-onsuccess-reload", onSuccessReload);
            return this;
        }

        public ActionBuilder confirm(string confirm)
        {
            this.HtmlAttributes.Add("if-ajax-onbefore-func", confirm);
            return this;
        }

        public ActionBuilder onBeforeFunc(string onBeforeFunc)
        {
            this.HtmlAttributes.Add("if-ajax-onbefore-func", onBeforeFunc);
            return this;
        }

        public ActionBuilder onCompleteFunc(string onCompleteFunc)
        {
            this.HtmlAttributes.Add("if-ajax-oncomplete-func", onCompleteFunc);
            return this;
        }

        public ActionBuilder onSuccessRefresh(string onSuccessRefresh)
        {
            this.HtmlAttributes.Add("if-ajax-on-success-refresh", onSuccessRefresh);
            return this;
        }

        public ActionBuilder onSuccessRefreshAction(string onSuccessRefreshAction)
        {
            this.HtmlAttributes.Add("if-ajax-on-success-refresh-action", onSuccessRefreshAction);
            return this;
        }

        public ActionBuilder onSuccessRefreshUpdateId(string onSuccessRefreshUpdateId)
        {
            this.HtmlAttributes.Add("if-ajax-on-success-refresh-updateid", onSuccessRefreshUpdateId);
            return this;
        }

        public ActionBuilder antiForgeryToken(string antiForgeryToken)
        {
            this.HtmlAttributes.Add("if-anti-forgery-token", antiForgeryToken);
            return this;
        }


        //updateid: $(element).attr("if-ajax-update-id"),
        //modalid: $(element).attr("if-ajax-modal-id"),
        //refreshGrid: $(element).attr("if-ajax-refresh-grid"),
        //showDialog: $(element).attr("if-ajax-show-dialog"),
        //insertionMode: $(element).attr("if-ajax-insertion-mode"),
        //method: $(element).attr("if-ajax-method"),
        //enctype: $(element).attr("if-ajax-enctype"),

        //CloseModalAfterSuccess: $(element).attr("if-ajax-close-modal-on-success"),

        //gridViewId: $(element).attr("if-ajax-gridview-id"),


        //onErrorFunc: $(element).attr("if-ajax-onerror-func"),


        //onSuccessFunc: $(element).attr("if-ajax-onsuccess-func"),


        //onSuccessReload: $(element).attr("if-ajax-onsuccess-reload"),

        //confirm: $(element).attr("if-ajax-confirm"),


        //onBeforeFunc: $(element).attr("if-ajax-onbefore-func"),

        //onCompleteFunc: $(element).attr("if-ajax-oncomplete-func"),

        //onSuccessRefresh: $(element).attr("if-ajax-on-success-refresh"),

        //onSuccessRefreshAction: $(element).attr("if-ajax-on-success-refresh-action"),
        //onSuccessRefreshUpdateId: $(element).attr("if-ajax-on-success-refresh-updateid"),
        //antiForgeryToken: $(element).attr("if-anti-forgery-token"),
    }
}


