/****************************************************
	文件：ProtoPackageCoder.cs
	作者：世界和平
	日期：2021/3/4 16:34:14
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rosiness.Network;
using System;

public class ProtoPackageCoder : DefaultNetworkPackageCoder
{
    public ProtoPackageCoder()
    {
        PackageSizeFieldType = EPackageSizeFieldType.UShort;
        MessageIDFieldType = EMessageIDFieldType.UShort;
    }

    protected override object DecodeInternal(Type classType, byte[] bodyBytes)
    {
        throw new NotImplementedException();
    }

    protected override byte[] EncodeInternal(object msgObj)
    {
        throw new NotImplementedException();
    }
}
