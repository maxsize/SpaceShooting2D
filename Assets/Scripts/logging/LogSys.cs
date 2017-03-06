using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using log4net.Config;
using log4net;
using log4net.Appender;
using System.IO;
using System;

public class LogSys : MonoBehaviour {
	// Use this for initialization
	void Start () {
		FileInfo file = new FileInfo(Application.dataPath + "/Plugins/log4net/log4net.xml");
		XmlConfigurator.ConfigureAndWatch(file);
	}

	public static ILog logger
	{
		get
		{
			if (Application.isMobilePlatform) return null;
			else return LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}