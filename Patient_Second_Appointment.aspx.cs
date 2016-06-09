﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Patient_Second_Appointment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string doc_name = toGetDoctorName();
        string appointment_date = toGetAppointmentDate();
        string serial_no = toGetSerialNo();

        Label_doc_name.Text = doc_name;
        Label_appointment_date.Text = appointment_date;
        Label_serial.Text = serial_no;
    }

    protected string toGetDoctorName()
    {
        string doc_name = null;
        string constr = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Doctor_Profile WHERE Doc_Email = @Doc_Email", con);
        con.Open();
        cmd.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        SqlDataReader dr;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            doc_name = Convert.ToString(dr[4]);
        }
        con.Close();


        return doc_name;
    }

    protected string toGetAppointmentDate()
    {
        string appointment_date = null;

        string constr1 = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
        SqlConnection con1 = new SqlConnection(constr1);
        SqlCommand cmd1 = new SqlCommand("SELECT * FROM Doctor_Serial WHERE Doc_Email = @Doc_Email", con1);
        con1.Open();
        cmd1.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        SqlDataReader dr1;
        dr1 = cmd1.ExecuteReader();

        int total_serial, current_serial;
        string serial_no = null;

        int[] ar = new int[100];
        int i = 0, take_date = 5000;
        while (dr1.Read())
        {
            DateTime d = Convert.ToDateTime(dr1[2]);
            int year = d.Year;
            int month = d.Month;
            int day = d.Day;
            ar[i] = year + month * 30 + day;
            if (ar[i] <= take_date)
            {
                appointment_date = Convert.ToString(dr1[2]);
                total_serial = Convert.ToInt32(dr1[3]);
                current_serial = Convert.ToInt32(dr1[4]);

                if (current_serial < total_serial)
                {
                    take_date = ar[i];
                    serial_no = (current_serial + 1).ToString();
                }
            }
            i++;
        }

        return appointment_date;
    }

    protected string toGetSerialNo()
    {
        string serial_no = null;

        string constr1 = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
        SqlConnection con1 = new SqlConnection(constr1);
        SqlCommand cmd1 = new SqlCommand("SELECT * FROM Doctor_Serial WHERE Doc_Email = @Doc_Email", con1);
        con1.Open();
        cmd1.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        SqlDataReader dr1;
        dr1 = cmd1.ExecuteReader();

        int total_serial, current_serial;
        string appointment_date = null;

        int[] ar = new int[100];
        int i = 0, take_date = 5000;
        while (dr1.Read())
        {
            DateTime d = Convert.ToDateTime(dr1[2]);
            int year = d.Year;
            int month = d.Month;
            int day = d.Day;
            ar[i] = year + month * 30 + day;
            if (ar[i] <= take_date)
            {
                appointment_date = Convert.ToString(dr1[2]);
                total_serial = Convert.ToInt32(dr1[3]);
                current_serial = Convert.ToInt32(dr1[4]);

                if (current_serial < total_serial)
                {
                    take_date = ar[i];
                    serial_no = (current_serial + 1).ToString();
                }
            }
            i++;
        }

        return serial_no;
    }

    protected void Logout(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetNoServerCaching();
        HttpContext.Current.Response.Cache.SetNoStore();
        Session.Abandon();
        Session.RemoveAll();
        Session.Clear();
        Response.Redirect("Patient.aspx");
    }

    protected string toGetNoVisit()
    {
        string no_visit = null;
        string constr = ConfigurationManager.ConnectionStrings["Prescription_ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Prescription WHERE Doc_Email = @Doc_Email AND Patient_Email = @Patient_Email", con);
        con.Open();
        cmd.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        cmd.Parameters.AddWithValue("@Patient_Email", Session["Patient_Email"].ToString());
        SqlDataReader dr;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            no_visit = Convert.ToString(dr[4]);
        }
        con.Close();


        return no_visit;
    }

    protected void Button_yes_Click(object sender, EventArgs e)
    {
        string appointment_date = toGetAppointmentDate();
        string serial_no = toGetSerialNo();
        string no_visit = toGetNoVisit();
        int temp = Convert.ToInt32(no_visit);
        temp = temp + 1;
        no_visit = Convert.ToString(temp);

        if (CheckBox_cash.Checked)
        {
            string constr = ConfigurationManager.ConnectionStrings["Prescription_ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("UPDATE Prescription SET Appointment_Date=@Appointment_Date, Serial_No=@Serial_No, Paid=@Paid, Payment_Method=@Payment_Method, No_Visit=@No_Visit WHERE Doc_Email=@Doc_Email AND Patient_Email=@Patient_Email", con);
            
            con.Open();
            cmd.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
            cmd.Parameters.AddWithValue("@Patient_Email", Session["Patient_Email"].ToString());
            cmd.Parameters.AddWithValue("@Appointment_Date", appointment_date);
            cmd.Parameters.AddWithValue("@Serial_No", serial_no);      
            cmd.Parameters.AddWithValue("@Paid", "No");
            cmd.Parameters.AddWithValue("@Payment_Method", "Cash");
            cmd.Parameters.AddWithValue("@No_Visit", no_visit);
            cmd.ExecuteNonQuery();
            con.Close();

            /*.......................Increase Current Serial In Doctor_Serial Database..........................*/

            string constr0 = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
            SqlConnection con0 = new SqlConnection(constr0);
            SqlCommand cmd0 = new SqlCommand("UPDATE Doctor_Serial SET Current_Serial=@Current_Serial WHERE Doc_Email = @Doc_Email AND Appointment_Date=@Appointment_Date", con0);
            con0.Open();
            cmd0.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
            cmd0.Parameters.AddWithValue("@Appointment_Date", appointment_date);
            cmd0.Parameters.AddWithValue("@Current_Serial", serial_no);
            cmd0.ExecuteNonQuery();
            con0.Close();

            Response.Redirect("Patient_Confirm.aspx");
        }

        else if (CheckBox_online.Checked)
        {
            Response.Redirect("Patient_Second_Online.aspx");
        }
        else
        {
            Label_warning.Visible = true;
            Label_warning.Text = "You have to select a payment method";
        }
    }



    protected void Button_no_Click(object sender, EventArgs e)
    {
        Response.Redirect("Patient_Profile.aspx");
    }
}