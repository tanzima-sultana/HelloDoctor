﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_View_LN : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Section"] = "Latest_News";
    }
}