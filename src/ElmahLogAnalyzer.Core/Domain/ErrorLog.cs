﻿using System;
using System.Collections.Generic;
using ElmahLogAnalyzer.Core.Common;
using ElmahLogAnalyzer.Core.Constants;

namespace ElmahLogAnalyzer.Core.Domain
{
	public class ErrorLog
	{
		private static readonly List<string> IgnoreList = new List<string> { "ALL_HTTP", "ALL_RAW" };

		public ErrorLog()
		{
			Url = "UNKNOWN";
			CleanUrl = Url;
			User = "UNKNOWN";
			ServerVariables = new List<NameValuePair>();
			Cookies = new List<NameValuePair>();
			FormValues = new List<NameValuePair>();
			QuerystringValues = new List<NameValuePair>();
			ClientInformation = new ClientInformation();
			StatusCodeInformation = new HttpStatusCodeInformation();
			CustomDataValues = new List<NameValuePair>();
		}

		public string Application { get; set; }

		public Guid ErrorId { get; set; }

		public string Host { get; set; }
		
		public string Type { get; set; }
		
		public string Message { get; set; }
		
		public string Source { get; set; }
		
		public string Details { get; set; }
		
		public DateTime Time { get; set; }

		public string StatusCode { get; set; }
		
		public string User { get; private set; }

		public string Url { get; private set; }

		public string CleanUrl { get; private set; }

		public string HttpUserAgent { get; private set; }

		public string ClientIpAddress { get; private set; }

		public List<NameValuePair> ServerVariables { get; private set; }

		public List<NameValuePair> QuerystringValues { get; private set; }

		public List<NameValuePair> FormValues { get; private set; }

		public List<NameValuePair> Cookies { get; private set; }

		public ClientInformation ClientInformation { get; private set; }

		public HttpStatusCodeInformation StatusCodeInformation { get; private set; }

		public ServerInformation ServerInformation { get; private set; }

		public List<NameValuePair> CustomDataValues { get; private set; }

		public void SetClientInformation(ClientInformation information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");	
			}

			ClientInformation = information;
		}

		public void SetStatusCodeInformation(HttpStatusCodeInformation information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}

			StatusCodeInformation = information;
		}

		public void SetServerInformation(ServerInformation information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}

			ServerInformation = information;
		}

		public void AddServerVariable(string name, string value)
		{
			if (!ShouldBeIncluded(name))
			{
				return;
			}

			if (name == HttpServerVariables.LogonUser && value.HasValue())
			{
				User = value.ToLowerInvariant();
			}
			
			if (name == HttpServerVariables.Url && value.HasValue())
			{
				Url = value.ToLowerInvariant();
				CleanUrl = UrlCleaner.Clean(Url);
			}

			if (name == HttpServerVariables.HttpUserAgent)
			{
				HttpUserAgent = value;
			}
			
			if (name == HttpServerVariables.RemoteAddress)
			{
				ClientIpAddress = value;
			}

			ServerVariables.Add(new NameValuePair(name, value));
		}

		public void AddQuerystringValue(string name, string value)
		{
			QuerystringValues.Add(new NameValuePair(name, value));
		}

		public void AddCustomDataValue(string name, string value)
		{
			CustomDataValues.Add(new NameValuePair(name, value));
		}

		public void AddFormValue(string name, string value)
		{
			FormValues.Add(new NameValuePair(name, value));
		}

		public void AddCookie(string name, string value)
		{
			Cookies.Add(new NameValuePair(name, value));
		}

		public override string ToString()
		{
			return string.Format("{0}\n{1}\n{2}\n{3}", Time, Source, Type, Message);
		}

		private static bool ShouldBeIncluded(string name)
		{
			return !IgnoreList.Contains(name);
		}
	}
}
