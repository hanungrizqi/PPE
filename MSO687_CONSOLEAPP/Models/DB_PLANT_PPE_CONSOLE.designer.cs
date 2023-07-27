﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSO687_CONSOLEAPP.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="DB_PLANT_PPE_NEW_KPT")]
	public partial class DB_PLANT_PPE_CONSOLEDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertTBL_T_PPE(TBL_T_PPE instance);
    partial void UpdateTBL_T_PPE(TBL_T_PPE instance);
    partial void DeleteTBL_T_PPE(TBL_T_PPE instance);
    #endregion
		
		public DB_PLANT_PPE_CONSOLEDataContext() : 
				base(global::MSO687_CONSOLEAPP.Properties.Settings.Default.DB_PLANT_PPE_NEW_KPTConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DB_PLANT_PPE_CONSOLEDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DB_PLANT_PPE_CONSOLEDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DB_PLANT_PPE_CONSOLEDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DB_PLANT_PPE_CONSOLEDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<VW_MSF685> VW_MSF685s
		{
			get
			{
				return this.GetTable<VW_MSF685>();
			}
		}
		
		public System.Data.Linq.Table<VW_R_ACCT_PROFILE> VW_R_ACCT_PROFILEs
		{
			get
			{
				return this.GetTable<VW_R_ACCT_PROFILE>();
			}
		}
		
		public System.Data.Linq.Table<TBL_T_PPE> TBL_T_PPEs
		{
			get
			{
				return this.GetTable<TBL_T_PPE>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.VW_MSF685")]
	public partial class VW_MSF685
	{
		
		private string _ASSET_NO;
		
		private char _ASSET_TY;
		
		private string _DSTRCT_CODE;
		
		private string _SUB_ASSET_NO;
		
		private string _ACCT_PROFILE;
		
		private string _ASSET_CLASSIF;
		
		private char _CAPITAL_TYPE;
		
		private string _CASH_GEN_UNIT;
		
		private string _CREATION_DATE;
		
		private string _CREATION_TIME;
		
		private string _CREATION_USER;
		
		private string _DEPR_EXP_CODE;
		
		private char _DEPR_IND;
		
		private string _DESC_CODEX1;
		
		private string _DESC_CODEX2;
		
		private string _DESC_CODEX3;
		
		private string _DESC_CODEX4;
		
		private string _DESC_CODEX5;
		
		private string _DESC_CODEX6;
		
		private string _DESC_CODEX7;
		
		private string _DESC_CODEX8;
		
		private string _LAST_MOD_DATE;
		
		private string _LAST_MOD_TIME;
		
		private string _LAST_MOD_USER;
		
		private string _MAA_CODE;
		
		private string _REPORT_CODE;
		
		private string _RETIREMENT_CODE;
		
		private string _RETIREMENT_DETL;
		
		private string _REVAL_SUB_CLASS;
		
		private char _REVIEWED_STATUS;
		
		private string _SUB_ASSET_DESC;
		
		private string _SUB_ASSET_STATUS;
		
		private string _TRACK_NUMBER;
		
		private char _TRACK_TYPE;
		
		private string _VINTAGE;
		
		private string _LAST_MOD_EMP;
		
		public VW_MSF685()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ASSET_NO", DbType="Char(12) NOT NULL", CanBeNull=false)]
		public string ASSET_NO
		{
			get
			{
				return this._ASSET_NO;
			}
			set
			{
				if ((this._ASSET_NO != value))
				{
					this._ASSET_NO = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ASSET_TY", DbType="Char(1) NOT NULL")]
		public char ASSET_TY
		{
			get
			{
				return this._ASSET_TY;
			}
			set
			{
				if ((this._ASSET_TY != value))
				{
					this._ASSET_TY = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DSTRCT_CODE", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string DSTRCT_CODE
		{
			get
			{
				return this._DSTRCT_CODE;
			}
			set
			{
				if ((this._DSTRCT_CODE != value))
				{
					this._DSTRCT_CODE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SUB_ASSET_NO", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string SUB_ASSET_NO
		{
			get
			{
				return this._SUB_ASSET_NO;
			}
			set
			{
				if ((this._SUB_ASSET_NO != value))
				{
					this._SUB_ASSET_NO = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ACCT_PROFILE", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string ACCT_PROFILE
		{
			get
			{
				return this._ACCT_PROFILE;
			}
			set
			{
				if ((this._ACCT_PROFILE != value))
				{
					this._ACCT_PROFILE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ASSET_CLASSIF", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string ASSET_CLASSIF
		{
			get
			{
				return this._ASSET_CLASSIF;
			}
			set
			{
				if ((this._ASSET_CLASSIF != value))
				{
					this._ASSET_CLASSIF = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CAPITAL_TYPE", DbType="Char(1) NOT NULL")]
		public char CAPITAL_TYPE
		{
			get
			{
				return this._CAPITAL_TYPE;
			}
			set
			{
				if ((this._CAPITAL_TYPE != value))
				{
					this._CAPITAL_TYPE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CASH_GEN_UNIT", DbType="Char(2) NOT NULL", CanBeNull=false)]
		public string CASH_GEN_UNIT
		{
			get
			{
				return this._CASH_GEN_UNIT;
			}
			set
			{
				if ((this._CASH_GEN_UNIT != value))
				{
					this._CASH_GEN_UNIT = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CREATION_DATE", DbType="Char(8) NOT NULL", CanBeNull=false)]
		public string CREATION_DATE
		{
			get
			{
				return this._CREATION_DATE;
			}
			set
			{
				if ((this._CREATION_DATE != value))
				{
					this._CREATION_DATE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CREATION_TIME", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string CREATION_TIME
		{
			get
			{
				return this._CREATION_TIME;
			}
			set
			{
				if ((this._CREATION_TIME != value))
				{
					this._CREATION_TIME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CREATION_USER", DbType="Char(10) NOT NULL", CanBeNull=false)]
		public string CREATION_USER
		{
			get
			{
				return this._CREATION_USER;
			}
			set
			{
				if ((this._CREATION_USER != value))
				{
					this._CREATION_USER = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DEPR_EXP_CODE", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string DEPR_EXP_CODE
		{
			get
			{
				return this._DEPR_EXP_CODE;
			}
			set
			{
				if ((this._DEPR_EXP_CODE != value))
				{
					this._DEPR_EXP_CODE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DEPR_IND", DbType="Char(1) NOT NULL")]
		public char DEPR_IND
		{
			get
			{
				return this._DEPR_IND;
			}
			set
			{
				if ((this._DEPR_IND != value))
				{
					this._DEPR_IND = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX1", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX1
		{
			get
			{
				return this._DESC_CODEX1;
			}
			set
			{
				if ((this._DESC_CODEX1 != value))
				{
					this._DESC_CODEX1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX2", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX2
		{
			get
			{
				return this._DESC_CODEX2;
			}
			set
			{
				if ((this._DESC_CODEX2 != value))
				{
					this._DESC_CODEX2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX3", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX3
		{
			get
			{
				return this._DESC_CODEX3;
			}
			set
			{
				if ((this._DESC_CODEX3 != value))
				{
					this._DESC_CODEX3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX4", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX4
		{
			get
			{
				return this._DESC_CODEX4;
			}
			set
			{
				if ((this._DESC_CODEX4 != value))
				{
					this._DESC_CODEX4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX5", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX5
		{
			get
			{
				return this._DESC_CODEX5;
			}
			set
			{
				if ((this._DESC_CODEX5 != value))
				{
					this._DESC_CODEX5 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX6", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX6
		{
			get
			{
				return this._DESC_CODEX6;
			}
			set
			{
				if ((this._DESC_CODEX6 != value))
				{
					this._DESC_CODEX6 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX7", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX7
		{
			get
			{
				return this._DESC_CODEX7;
			}
			set
			{
				if ((this._DESC_CODEX7 != value))
				{
					this._DESC_CODEX7 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESC_CODEX8", DbType="Char(18) NOT NULL", CanBeNull=false)]
		public string DESC_CODEX8
		{
			get
			{
				return this._DESC_CODEX8;
			}
			set
			{
				if ((this._DESC_CODEX8 != value))
				{
					this._DESC_CODEX8 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LAST_MOD_DATE", DbType="Char(8) NOT NULL", CanBeNull=false)]
		public string LAST_MOD_DATE
		{
			get
			{
				return this._LAST_MOD_DATE;
			}
			set
			{
				if ((this._LAST_MOD_DATE != value))
				{
					this._LAST_MOD_DATE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LAST_MOD_TIME", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string LAST_MOD_TIME
		{
			get
			{
				return this._LAST_MOD_TIME;
			}
			set
			{
				if ((this._LAST_MOD_TIME != value))
				{
					this._LAST_MOD_TIME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LAST_MOD_USER", DbType="Char(10) NOT NULL", CanBeNull=false)]
		public string LAST_MOD_USER
		{
			get
			{
				return this._LAST_MOD_USER;
			}
			set
			{
				if ((this._LAST_MOD_USER != value))
				{
					this._LAST_MOD_USER = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MAA_CODE", DbType="Char(24) NOT NULL", CanBeNull=false)]
		public string MAA_CODE
		{
			get
			{
				return this._MAA_CODE;
			}
			set
			{
				if ((this._MAA_CODE != value))
				{
					this._MAA_CODE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_REPORT_CODE", DbType="Char(15) NOT NULL", CanBeNull=false)]
		public string REPORT_CODE
		{
			get
			{
				return this._REPORT_CODE;
			}
			set
			{
				if ((this._REPORT_CODE != value))
				{
					this._REPORT_CODE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RETIREMENT_CODE", DbType="Char(2) NOT NULL", CanBeNull=false)]
		public string RETIREMENT_CODE
		{
			get
			{
				return this._RETIREMENT_CODE;
			}
			set
			{
				if ((this._RETIREMENT_CODE != value))
				{
					this._RETIREMENT_CODE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RETIREMENT_DETL", DbType="Char(40) NOT NULL", CanBeNull=false)]
		public string RETIREMENT_DETL
		{
			get
			{
				return this._RETIREMENT_DETL;
			}
			set
			{
				if ((this._RETIREMENT_DETL != value))
				{
					this._RETIREMENT_DETL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_REVAL_SUB_CLASS", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string REVAL_SUB_CLASS
		{
			get
			{
				return this._REVAL_SUB_CLASS;
			}
			set
			{
				if ((this._REVAL_SUB_CLASS != value))
				{
					this._REVAL_SUB_CLASS = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_REVIEWED_STATUS", DbType="Char(1) NOT NULL")]
		public char REVIEWED_STATUS
		{
			get
			{
				return this._REVIEWED_STATUS;
			}
			set
			{
				if ((this._REVIEWED_STATUS != value))
				{
					this._REVIEWED_STATUS = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SUB_ASSET_DESC", DbType="Char(40) NOT NULL", CanBeNull=false)]
		public string SUB_ASSET_DESC
		{
			get
			{
				return this._SUB_ASSET_DESC;
			}
			set
			{
				if ((this._SUB_ASSET_DESC != value))
				{
					this._SUB_ASSET_DESC = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SUB_ASSET_STATUS", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string SUB_ASSET_STATUS
		{
			get
			{
				return this._SUB_ASSET_STATUS;
			}
			set
			{
				if ((this._SUB_ASSET_STATUS != value))
				{
					this._SUB_ASSET_STATUS = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TRACK_NUMBER", DbType="Char(30) NOT NULL", CanBeNull=false)]
		public string TRACK_NUMBER
		{
			get
			{
				return this._TRACK_NUMBER;
			}
			set
			{
				if ((this._TRACK_NUMBER != value))
				{
					this._TRACK_NUMBER = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TRACK_TYPE", DbType="Char(1) NOT NULL")]
		public char TRACK_TYPE
		{
			get
			{
				return this._TRACK_TYPE;
			}
			set
			{
				if ((this._TRACK_TYPE != value))
				{
					this._TRACK_TYPE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VINTAGE", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string VINTAGE
		{
			get
			{
				return this._VINTAGE;
			}
			set
			{
				if ((this._VINTAGE != value))
				{
					this._VINTAGE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LAST_MOD_EMP", DbType="Char(10) NOT NULL", CanBeNull=false)]
		public string LAST_MOD_EMP
		{
			get
			{
				return this._LAST_MOD_EMP;
			}
			set
			{
				if ((this._LAST_MOD_EMP != value))
				{
					this._LAST_MOD_EMP = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.VW_R_ACCT_PROFILE")]
	public partial class VW_R_ACCT_PROFILE
	{
		
		private string _EQUIP_NO;
		
		private string _DSTRCT_CODE;
		
		private string _EQUIP_LOCATION;
		
		private string _SUB_ASSET_NO;
		
		private string _ACCT_PROFILE;
		
		public VW_R_ACCT_PROFILE()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EQUIP_NO", DbType="Char(12) NOT NULL", CanBeNull=false)]
		public string EQUIP_NO
		{
			get
			{
				return this._EQUIP_NO;
			}
			set
			{
				if ((this._EQUIP_NO != value))
				{
					this._EQUIP_NO = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DSTRCT_CODE", DbType="Char(4) NOT NULL", CanBeNull=false)]
		public string DSTRCT_CODE
		{
			get
			{
				return this._DSTRCT_CODE;
			}
			set
			{
				if ((this._DSTRCT_CODE != value))
				{
					this._DSTRCT_CODE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EQUIP_LOCATION", DbType="Char(5) NOT NULL", CanBeNull=false)]
		public string EQUIP_LOCATION
		{
			get
			{
				return this._EQUIP_LOCATION;
			}
			set
			{
				if ((this._EQUIP_LOCATION != value))
				{
					this._EQUIP_LOCATION = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SUB_ASSET_NO", DbType="Char(6)")]
		public string SUB_ASSET_NO
		{
			get
			{
				return this._SUB_ASSET_NO;
			}
			set
			{
				if ((this._SUB_ASSET_NO != value))
				{
					this._SUB_ASSET_NO = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ACCT_PROFILE", DbType="Char(4)")]
		public string ACCT_PROFILE
		{
			get
			{
				return this._ACCT_PROFILE;
			}
			set
			{
				if ((this._ACCT_PROFILE != value))
				{
					this._ACCT_PROFILE = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TBL_T_PPE")]
	public partial class TBL_T_PPE : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ID_PPE;
		
		private int _ID;
		
		private System.Nullable<int> _APPROVAL_ORDER;
		
		private string _PPE_NO;
		
		private System.Nullable<System.DateTime> _DATE;
		
		private string _DISTRICT_FROM;
		
		private string _DISTRICT_TO;
		
		private string _LOC_FROM;
		
		private string _LOC_TO;
		
		private string _EQUIP_NO;
		
		private string _EGI;
		
		private string _EQUIP_CLASS;
		
		private string _SERIAL_NO;
		
		private string _PPE_DESC;
		
		private string _POSISI_PPE;
		
		private string _STATUS;
		
		private System.Nullable<System.DateTime> _CREATED_DATE;
		
		private string _CREATED_BY;
		
		private string _CREATED_POS_BY;
		
		private System.Nullable<System.DateTime> _UPDATED_DATE;
		
		private string _UPDATED_BY;
		
		private string _REMARKS;
		
		private string _PATH_ATTACHMENT;
		
		private string _UPLOAD_FORM_CAAB;
		
		private string _URL_FORM_SH;
		
		private string _URL_FORM_PLNTMNGR;
		
		private string _URL_FORM_PLNTDH;
		
		private string _URL_FORM_PM_PENGIRIM;
		
		private string _URL_FORM_PM_PENERIMA;
		
		private string _URL_FORM_DIVHEAD_ENG;
		
		private string _URL_FORM_DIVHEAD_OPR;
		
		private string _URL_FORM_DONE;
		
		private System.Nullable<System.DateTime> _DATE_RECEIVED_SM;
		
		private string _BERITA_ACARA_SM;
		
		private System.Nullable<int> _FLAG;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnID_PPEChanging(System.Guid value);
    partial void OnID_PPEChanged();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnAPPROVAL_ORDERChanging(System.Nullable<int> value);
    partial void OnAPPROVAL_ORDERChanged();
    partial void OnPPE_NOChanging(string value);
    partial void OnPPE_NOChanged();
    partial void OnDATEChanging(System.Nullable<System.DateTime> value);
    partial void OnDATEChanged();
    partial void OnDISTRICT_FROMChanging(string value);
    partial void OnDISTRICT_FROMChanged();
    partial void OnDISTRICT_TOChanging(string value);
    partial void OnDISTRICT_TOChanged();
    partial void OnLOC_FROMChanging(string value);
    partial void OnLOC_FROMChanged();
    partial void OnLOC_TOChanging(string value);
    partial void OnLOC_TOChanged();
    partial void OnEQUIP_NOChanging(string value);
    partial void OnEQUIP_NOChanged();
    partial void OnEGIChanging(string value);
    partial void OnEGIChanged();
    partial void OnEQUIP_CLASSChanging(string value);
    partial void OnEQUIP_CLASSChanged();
    partial void OnSERIAL_NOChanging(string value);
    partial void OnSERIAL_NOChanged();
    partial void OnPPE_DESCChanging(string value);
    partial void OnPPE_DESCChanged();
    partial void OnPOSISI_PPEChanging(string value);
    partial void OnPOSISI_PPEChanged();
    partial void OnSTATUSChanging(string value);
    partial void OnSTATUSChanged();
    partial void OnCREATED_DATEChanging(System.Nullable<System.DateTime> value);
    partial void OnCREATED_DATEChanged();
    partial void OnCREATED_BYChanging(string value);
    partial void OnCREATED_BYChanged();
    partial void OnCREATED_POS_BYChanging(string value);
    partial void OnCREATED_POS_BYChanged();
    partial void OnUPDATED_DATEChanging(System.Nullable<System.DateTime> value);
    partial void OnUPDATED_DATEChanged();
    partial void OnUPDATED_BYChanging(string value);
    partial void OnUPDATED_BYChanged();
    partial void OnREMARKSChanging(string value);
    partial void OnREMARKSChanged();
    partial void OnPATH_ATTACHMENTChanging(string value);
    partial void OnPATH_ATTACHMENTChanged();
    partial void OnUPLOAD_FORM_CAABChanging(string value);
    partial void OnUPLOAD_FORM_CAABChanged();
    partial void OnURL_FORM_SHChanging(string value);
    partial void OnURL_FORM_SHChanged();
    partial void OnURL_FORM_PLNTMNGRChanging(string value);
    partial void OnURL_FORM_PLNTMNGRChanged();
    partial void OnURL_FORM_PLNTDHChanging(string value);
    partial void OnURL_FORM_PLNTDHChanged();
    partial void OnURL_FORM_PM_PENGIRIMChanging(string value);
    partial void OnURL_FORM_PM_PENGIRIMChanged();
    partial void OnURL_FORM_PM_PENERIMAChanging(string value);
    partial void OnURL_FORM_PM_PENERIMAChanged();
    partial void OnURL_FORM_DIVHEAD_ENGChanging(string value);
    partial void OnURL_FORM_DIVHEAD_ENGChanged();
    partial void OnURL_FORM_DIVHEAD_OPRChanging(string value);
    partial void OnURL_FORM_DIVHEAD_OPRChanged();
    partial void OnURL_FORM_DONEChanging(string value);
    partial void OnURL_FORM_DONEChanged();
    partial void OnDATE_RECEIVED_SMChanging(System.Nullable<System.DateTime> value);
    partial void OnDATE_RECEIVED_SMChanged();
    partial void OnBERITA_ACARA_SMChanging(string value);
    partial void OnBERITA_ACARA_SMChanged();
    partial void OnFLAGChanging(System.Nullable<int> value);
    partial void OnFLAGChanged();
    #endregion
		
		public TBL_T_PPE()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID_PPE", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ID_PPE
		{
			get
			{
				return this._ID_PPE;
			}
			set
			{
				if ((this._ID_PPE != value))
				{
					this.OnID_PPEChanging(value);
					this.SendPropertyChanging();
					this._ID_PPE = value;
					this.SendPropertyChanged("ID_PPE");
					this.OnID_PPEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.Always, DbType="Int NOT NULL IDENTITY", IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_APPROVAL_ORDER", DbType="Int")]
		public System.Nullable<int> APPROVAL_ORDER
		{
			get
			{
				return this._APPROVAL_ORDER;
			}
			set
			{
				if ((this._APPROVAL_ORDER != value))
				{
					this.OnAPPROVAL_ORDERChanging(value);
					this.SendPropertyChanging();
					this._APPROVAL_ORDER = value;
					this.SendPropertyChanged("APPROVAL_ORDER");
					this.OnAPPROVAL_ORDERChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PPE_NO", DbType="VarChar(200)")]
		public string PPE_NO
		{
			get
			{
				return this._PPE_NO;
			}
			set
			{
				if ((this._PPE_NO != value))
				{
					this.OnPPE_NOChanging(value);
					this.SendPropertyChanging();
					this._PPE_NO = value;
					this.SendPropertyChanged("PPE_NO");
					this.OnPPE_NOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DATE", DbType="Date")]
		public System.Nullable<System.DateTime> DATE
		{
			get
			{
				return this._DATE;
			}
			set
			{
				if ((this._DATE != value))
				{
					this.OnDATEChanging(value);
					this.SendPropertyChanging();
					this._DATE = value;
					this.SendPropertyChanged("DATE");
					this.OnDATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DISTRICT_FROM", DbType="VarChar(50)")]
		public string DISTRICT_FROM
		{
			get
			{
				return this._DISTRICT_FROM;
			}
			set
			{
				if ((this._DISTRICT_FROM != value))
				{
					this.OnDISTRICT_FROMChanging(value);
					this.SendPropertyChanging();
					this._DISTRICT_FROM = value;
					this.SendPropertyChanged("DISTRICT_FROM");
					this.OnDISTRICT_FROMChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DISTRICT_TO", DbType="VarChar(50)")]
		public string DISTRICT_TO
		{
			get
			{
				return this._DISTRICT_TO;
			}
			set
			{
				if ((this._DISTRICT_TO != value))
				{
					this.OnDISTRICT_TOChanging(value);
					this.SendPropertyChanging();
					this._DISTRICT_TO = value;
					this.SendPropertyChanged("DISTRICT_TO");
					this.OnDISTRICT_TOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LOC_FROM", DbType="VarChar(50)")]
		public string LOC_FROM
		{
			get
			{
				return this._LOC_FROM;
			}
			set
			{
				if ((this._LOC_FROM != value))
				{
					this.OnLOC_FROMChanging(value);
					this.SendPropertyChanging();
					this._LOC_FROM = value;
					this.SendPropertyChanged("LOC_FROM");
					this.OnLOC_FROMChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LOC_TO", DbType="VarChar(50)")]
		public string LOC_TO
		{
			get
			{
				return this._LOC_TO;
			}
			set
			{
				if ((this._LOC_TO != value))
				{
					this.OnLOC_TOChanging(value);
					this.SendPropertyChanging();
					this._LOC_TO = value;
					this.SendPropertyChanged("LOC_TO");
					this.OnLOC_TOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EQUIP_NO", DbType="VarChar(50)")]
		public string EQUIP_NO
		{
			get
			{
				return this._EQUIP_NO;
			}
			set
			{
				if ((this._EQUIP_NO != value))
				{
					this.OnEQUIP_NOChanging(value);
					this.SendPropertyChanging();
					this._EQUIP_NO = value;
					this.SendPropertyChanged("EQUIP_NO");
					this.OnEQUIP_NOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EGI", DbType="VarChar(20)")]
		public string EGI
		{
			get
			{
				return this._EGI;
			}
			set
			{
				if ((this._EGI != value))
				{
					this.OnEGIChanging(value);
					this.SendPropertyChanging();
					this._EGI = value;
					this.SendPropertyChanged("EGI");
					this.OnEGIChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EQUIP_CLASS", DbType="VarChar(10)")]
		public string EQUIP_CLASS
		{
			get
			{
				return this._EQUIP_CLASS;
			}
			set
			{
				if ((this._EQUIP_CLASS != value))
				{
					this.OnEQUIP_CLASSChanging(value);
					this.SendPropertyChanging();
					this._EQUIP_CLASS = value;
					this.SendPropertyChanged("EQUIP_CLASS");
					this.OnEQUIP_CLASSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SERIAL_NO", DbType="VarChar(50)")]
		public string SERIAL_NO
		{
			get
			{
				return this._SERIAL_NO;
			}
			set
			{
				if ((this._SERIAL_NO != value))
				{
					this.OnSERIAL_NOChanging(value);
					this.SendPropertyChanging();
					this._SERIAL_NO = value;
					this.SendPropertyChanged("SERIAL_NO");
					this.OnSERIAL_NOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PPE_DESC", DbType="VarChar(200)")]
		public string PPE_DESC
		{
			get
			{
				return this._PPE_DESC;
			}
			set
			{
				if ((this._PPE_DESC != value))
				{
					this.OnPPE_DESCChanging(value);
					this.SendPropertyChanging();
					this._PPE_DESC = value;
					this.SendPropertyChanged("PPE_DESC");
					this.OnPPE_DESCChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_POSISI_PPE", DbType="VarChar(100)")]
		public string POSISI_PPE
		{
			get
			{
				return this._POSISI_PPE;
			}
			set
			{
				if ((this._POSISI_PPE != value))
				{
					this.OnPOSISI_PPEChanging(value);
					this.SendPropertyChanging();
					this._POSISI_PPE = value;
					this.SendPropertyChanged("POSISI_PPE");
					this.OnPOSISI_PPEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_STATUS", DbType="VarChar(50)")]
		public string STATUS
		{
			get
			{
				return this._STATUS;
			}
			set
			{
				if ((this._STATUS != value))
				{
					this.OnSTATUSChanging(value);
					this.SendPropertyChanging();
					this._STATUS = value;
					this.SendPropertyChanged("STATUS");
					this.OnSTATUSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CREATED_DATE", DbType="DateTime")]
		public System.Nullable<System.DateTime> CREATED_DATE
		{
			get
			{
				return this._CREATED_DATE;
			}
			set
			{
				if ((this._CREATED_DATE != value))
				{
					this.OnCREATED_DATEChanging(value);
					this.SendPropertyChanging();
					this._CREATED_DATE = value;
					this.SendPropertyChanged("CREATED_DATE");
					this.OnCREATED_DATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CREATED_BY", DbType="VarChar(50)")]
		public string CREATED_BY
		{
			get
			{
				return this._CREATED_BY;
			}
			set
			{
				if ((this._CREATED_BY != value))
				{
					this.OnCREATED_BYChanging(value);
					this.SendPropertyChanging();
					this._CREATED_BY = value;
					this.SendPropertyChanged("CREATED_BY");
					this.OnCREATED_BYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CREATED_POS_BY", DbType="VarChar(50)")]
		public string CREATED_POS_BY
		{
			get
			{
				return this._CREATED_POS_BY;
			}
			set
			{
				if ((this._CREATED_POS_BY != value))
				{
					this.OnCREATED_POS_BYChanging(value);
					this.SendPropertyChanging();
					this._CREATED_POS_BY = value;
					this.SendPropertyChanged("CREATED_POS_BY");
					this.OnCREATED_POS_BYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UPDATED_DATE", DbType="Date")]
		public System.Nullable<System.DateTime> UPDATED_DATE
		{
			get
			{
				return this._UPDATED_DATE;
			}
			set
			{
				if ((this._UPDATED_DATE != value))
				{
					this.OnUPDATED_DATEChanging(value);
					this.SendPropertyChanging();
					this._UPDATED_DATE = value;
					this.SendPropertyChanged("UPDATED_DATE");
					this.OnUPDATED_DATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UPDATED_BY", DbType="VarChar(50)")]
		public string UPDATED_BY
		{
			get
			{
				return this._UPDATED_BY;
			}
			set
			{
				if ((this._UPDATED_BY != value))
				{
					this.OnUPDATED_BYChanging(value);
					this.SendPropertyChanging();
					this._UPDATED_BY = value;
					this.SendPropertyChanged("UPDATED_BY");
					this.OnUPDATED_BYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_REMARKS", DbType="VarChar(MAX)")]
		public string REMARKS
		{
			get
			{
				return this._REMARKS;
			}
			set
			{
				if ((this._REMARKS != value))
				{
					this.OnREMARKSChanging(value);
					this.SendPropertyChanging();
					this._REMARKS = value;
					this.SendPropertyChanged("REMARKS");
					this.OnREMARKSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PATH_ATTACHMENT", DbType="VarChar(200)")]
		public string PATH_ATTACHMENT
		{
			get
			{
				return this._PATH_ATTACHMENT;
			}
			set
			{
				if ((this._PATH_ATTACHMENT != value))
				{
					this.OnPATH_ATTACHMENTChanging(value);
					this.SendPropertyChanging();
					this._PATH_ATTACHMENT = value;
					this.SendPropertyChanged("PATH_ATTACHMENT");
					this.OnPATH_ATTACHMENTChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UPLOAD_FORM_CAAB", DbType="VarChar(200)")]
		public string UPLOAD_FORM_CAAB
		{
			get
			{
				return this._UPLOAD_FORM_CAAB;
			}
			set
			{
				if ((this._UPLOAD_FORM_CAAB != value))
				{
					this.OnUPLOAD_FORM_CAABChanging(value);
					this.SendPropertyChanging();
					this._UPLOAD_FORM_CAAB = value;
					this.SendPropertyChanged("UPLOAD_FORM_CAAB");
					this.OnUPLOAD_FORM_CAABChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_SH", DbType="VarChar(100)")]
		public string URL_FORM_SH
		{
			get
			{
				return this._URL_FORM_SH;
			}
			set
			{
				if ((this._URL_FORM_SH != value))
				{
					this.OnURL_FORM_SHChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_SH = value;
					this.SendPropertyChanged("URL_FORM_SH");
					this.OnURL_FORM_SHChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_PLNTMNGR", DbType="VarChar(100)")]
		public string URL_FORM_PLNTMNGR
		{
			get
			{
				return this._URL_FORM_PLNTMNGR;
			}
			set
			{
				if ((this._URL_FORM_PLNTMNGR != value))
				{
					this.OnURL_FORM_PLNTMNGRChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_PLNTMNGR = value;
					this.SendPropertyChanged("URL_FORM_PLNTMNGR");
					this.OnURL_FORM_PLNTMNGRChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_PLNTDH", DbType="VarChar(100)")]
		public string URL_FORM_PLNTDH
		{
			get
			{
				return this._URL_FORM_PLNTDH;
			}
			set
			{
				if ((this._URL_FORM_PLNTDH != value))
				{
					this.OnURL_FORM_PLNTDHChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_PLNTDH = value;
					this.SendPropertyChanged("URL_FORM_PLNTDH");
					this.OnURL_FORM_PLNTDHChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_PM_PENGIRIM", DbType="VarChar(100)")]
		public string URL_FORM_PM_PENGIRIM
		{
			get
			{
				return this._URL_FORM_PM_PENGIRIM;
			}
			set
			{
				if ((this._URL_FORM_PM_PENGIRIM != value))
				{
					this.OnURL_FORM_PM_PENGIRIMChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_PM_PENGIRIM = value;
					this.SendPropertyChanged("URL_FORM_PM_PENGIRIM");
					this.OnURL_FORM_PM_PENGIRIMChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_PM_PENERIMA", DbType="VarChar(100)")]
		public string URL_FORM_PM_PENERIMA
		{
			get
			{
				return this._URL_FORM_PM_PENERIMA;
			}
			set
			{
				if ((this._URL_FORM_PM_PENERIMA != value))
				{
					this.OnURL_FORM_PM_PENERIMAChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_PM_PENERIMA = value;
					this.SendPropertyChanged("URL_FORM_PM_PENERIMA");
					this.OnURL_FORM_PM_PENERIMAChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_DIVHEAD_ENG", DbType="VarChar(100)")]
		public string URL_FORM_DIVHEAD_ENG
		{
			get
			{
				return this._URL_FORM_DIVHEAD_ENG;
			}
			set
			{
				if ((this._URL_FORM_DIVHEAD_ENG != value))
				{
					this.OnURL_FORM_DIVHEAD_ENGChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_DIVHEAD_ENG = value;
					this.SendPropertyChanged("URL_FORM_DIVHEAD_ENG");
					this.OnURL_FORM_DIVHEAD_ENGChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_DIVHEAD_OPR", DbType="VarChar(100)")]
		public string URL_FORM_DIVHEAD_OPR
		{
			get
			{
				return this._URL_FORM_DIVHEAD_OPR;
			}
			set
			{
				if ((this._URL_FORM_DIVHEAD_OPR != value))
				{
					this.OnURL_FORM_DIVHEAD_OPRChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_DIVHEAD_OPR = value;
					this.SendPropertyChanged("URL_FORM_DIVHEAD_OPR");
					this.OnURL_FORM_DIVHEAD_OPRChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL_FORM_DONE", DbType="VarChar(100)")]
		public string URL_FORM_DONE
		{
			get
			{
				return this._URL_FORM_DONE;
			}
			set
			{
				if ((this._URL_FORM_DONE != value))
				{
					this.OnURL_FORM_DONEChanging(value);
					this.SendPropertyChanging();
					this._URL_FORM_DONE = value;
					this.SendPropertyChanged("URL_FORM_DONE");
					this.OnURL_FORM_DONEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DATE_RECEIVED_SM", DbType="Date")]
		public System.Nullable<System.DateTime> DATE_RECEIVED_SM
		{
			get
			{
				return this._DATE_RECEIVED_SM;
			}
			set
			{
				if ((this._DATE_RECEIVED_SM != value))
				{
					this.OnDATE_RECEIVED_SMChanging(value);
					this.SendPropertyChanging();
					this._DATE_RECEIVED_SM = value;
					this.SendPropertyChanged("DATE_RECEIVED_SM");
					this.OnDATE_RECEIVED_SMChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BERITA_ACARA_SM", DbType="VarChar(MAX)")]
		public string BERITA_ACARA_SM
		{
			get
			{
				return this._BERITA_ACARA_SM;
			}
			set
			{
				if ((this._BERITA_ACARA_SM != value))
				{
					this.OnBERITA_ACARA_SMChanging(value);
					this.SendPropertyChanging();
					this._BERITA_ACARA_SM = value;
					this.SendPropertyChanged("BERITA_ACARA_SM");
					this.OnBERITA_ACARA_SMChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FLAG", DbType="Int")]
		public System.Nullable<int> FLAG
		{
			get
			{
				return this._FLAG;
			}
			set
			{
				if ((this._FLAG != value))
				{
					this.OnFLAGChanging(value);
					this.SendPropertyChanging();
					this._FLAG = value;
					this.SendPropertyChanged("FLAG");
					this.OnFLAGChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
