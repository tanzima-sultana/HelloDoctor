﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Patient_Second_Online : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string amount = toGetAmount();
        Label_amount.Text = amount;
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

    protected string toGetPatientName()
    {
        string patient_name = null;

        string constr = ConfigurationManager.ConnectionStrings["Patient_Registration_ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Patient_Registration WHERE Patient_Email = @Patient_Email", con);
        con.Open();
        cmd.Parameters.AddWithValue("@Patient_Email", Session["Patient_Email"].ToString());
        SqlDataReader dr;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            patient_name = Convert.ToString(dr[1]);
        }
        con.Close();

        return patient_name;
    }

    protected string toGetDoctorBank()
    {
        string doc_bank = null;

        string constr = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Doctor_Profile WHERE Doc_Email = @Doc_Email", con);
        con.Open();
        cmd.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        SqlDataReader dr;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            doc_bank = Convert.ToString(dr[20]);
        }
        con.Close();

        return doc_bank;
    }

    protected string toDoctorAccount()
    {
        string doc_account = null;

        string constr = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Doctor_Profile WHERE Doc_Email = @Doc_Email", con);
        con.Open();
        cmd.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        SqlDataReader dr;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            doc_account = Convert.ToString(dr[21]);
        }
        con.Close();

        return doc_account;
    }

    protected string toGetAmount()
    {
        string amount = null;

        string constr = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Doctor_Profile WHERE Doc_Email = @Doc_Email", con);
        con.Open();
        cmd.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        SqlDataReader dr;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            amount = Convert.ToString(dr[18]);
        }
        con.Close();

        return amount;
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

        string constr = ConfigurationManager.ConnectionStrings["Prescription_ConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("UPDATE Prescription SET Appointment_Date=@Appointment_Date, Serial_No=@Serial_No, Paid=@Paid, Payment_Method=@Payment_Method, No_Visit=@No_Visit WHERE Doc_Email=@Doc_Email AND Patient_Email=@Patient_Email", con);

        con.Open();
        cmd.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        cmd.Parameters.AddWithValue("@Patient_Email", Session["Patient_Email"].ToString());
        cmd.Parameters.AddWithValue("@Appointment_Date", appointment_date);
        cmd.Parameters.AddWithValue("@Serial_No", serial_no);
        cmd.Parameters.AddWithValue("@Paid", "No");
        cmd.Parameters.AddWithValue("@Payment_Method", "Online");
        cmd.Parameters.AddWithValue("@No_Visit", no_visit);
        cmd.ExecuteNonQuery();
        con.Close();

        /*........................Online_Payment database.............................................*/

        string patient_name = toGetPatientName();
        string doc_name = toGetDoctorName();
        string doc_bank = toGetDoctorBank();
        string doc_account = toDoctorAccount();
        string amount = toGetAmount();

        string constr1 = ConfigurationManager.ConnectionStrings["Online_Payment_ConnectionString"].ConnectionString;
        SqlConnection con1 = new SqlConnection(constr1);
        SqlCommand cmd1 = new SqlCommand("INSERT INTO Online_Payment (Doc_Email, Doc_Name, Doc_Bank, Doc_Account_No, Patient_Email, Patient_Name, Patient_Bank, Patient_Account_No, Patient_Card, Patient_Card_No, Amount, Date) values(@Doc_Email, @Doc_Name, @Doc_Bank, @Doc_Account_No, @Patient_Email, @Patient_Name, @Patient_Bank, @Patient_Account_No, @Patient_Card, @Patient_Card_No, @Amount, @Date)", con1);

        con1.Open();
        cmd1.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        cmd1.Parameters.AddWithValue("@Doc_Name", doc_name);
        cmd1.Parameters.AddWithValue("@Doc_Bank", doc_bank);
        cmd1.Parameters.AddWithValue("@Doc_Account_No", doc_account);
        cmd1.Parameters.AddWithValue("@Patient_Email", Session["Patient_Email"].ToString());
        cmd1.Parameters.AddWithValue("@Patient_Name", patient_name);
        cmd1.Parameters.AddWithValue("@Patient_Bank", TextBox_bank.Text);
        cmd1.Parameters.AddWithValue("@Patient_Account_No", TextBox_account_no.Text);
        cmd1.Parameters.AddWithValue("@Patient_Card", DropDownList_pay.SelectedItem.ToString());
        cmd1.Parameters.AddWithValue("@Patient_Card_No", TextBox_card_no.Text);
        cmd1.Parameters.AddWithValue("@Amount", amount);
        cmd1.Parameters.AddWithValue("@Date", DateTime.Now.ToShortDateString());
        cmd1.ExecuteNonQuery();
        con1.Close();

        int serial_db = Convert.ToInt32(serial_no);
        string serial_update = serial_db.ToString();

        string constr0 = ConfigurationManager.ConnectionStrings["Doctor_ConnectionString"].ConnectionString;
        SqlConnection con0 = new SqlConnection(constr0);
        SqlCommand cmd0 = new SqlCommand("UPDATE Doctor_Serial SET Current_Serial=@Current_Serial WHERE Doc_Email = @Doc_Email AND Appointment_Date=@Appointment_Date", con0);
        con0.Open();
        cmd0.Parameters.AddWithValue("@Doc_Email", Session["Doc_Email"].ToString());
        cmd0.Parameters.AddWithValue("@Appointment_Date", appointment_date);
        cmd0.Parameters.AddWithValue("@Current_Serial", serial_update);
        cmd0.ExecuteNonQuery();
        con0.Close();

        Session.Remove("Doc_Email");
        Response.Redirect("Patient_Confirm.aspx");
    }
    protected void Button_no_Click(object sender, EventArgs e)
    {
        Response.Redirect("Patient_Profile.aspx");
    }
}