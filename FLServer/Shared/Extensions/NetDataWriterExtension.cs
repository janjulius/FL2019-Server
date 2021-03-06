﻿using LiteNetLib.Utils;
using Shared.Attributes;
using Shared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    [Author("Jan Julius de Lang", version = 1.0)]
    public static class NetDataWriterExtension
    {
        public static void PutPacketStruct<T>(this NetDataWriter ndw, T packet) where T : struct
        {
            ndw.Put(packet.ToByteArray());
        }
        
        [Obsolete("Is obsolete use PutPacketStruct instead")]
        public static void PutPacket<T>(this NetDataWriter ndw, T packet) where T : Packet
        {
            foreach(var property in packet.GetType().GetProperties())
            {
                Console.WriteLine($"{property} is {property}" );
                dynamic variable = property.GetValue(packet);
                if (property.PropertyType.IsArray)
                {
                    Console.WriteLine($"{property} is array");
                    PutPackets(ndw, variable);
                }
                PutVariable(property.PropertyType, ndw, false, variable);
            }
        }

        [Obsolete("Is obsolete use PutPacketStruct instead")]
        public static void PutPackets<T>(this NetDataWriter ndw, T[] packets) where T : Packet
        {
            if (packets.Length > 0)
            {
                int propertyCount = packets[0].GetPropertySize();
                int propertyIndex = 0;
                foreach(PropertyInfo property in packets[0].GetType().GetProperties())
                {
                    dynamic arr = new dynamic[packets.Length];
                    int arrIndex = 0;
                    foreach(T packet in packets)
                    {
                        arr[arrIndex] = property.GetValue(packet, null);
                        arrIndex++;
                    }
                    PutVariable(property.PropertyType, ndw, true, arr);
                    propertyIndex++;
                }
            }
            else
            {
                throw new NotImplementedException("Packet cannot be empty");
            }
        }

        private static void PutVariable(Type t, NetDataWriter ndw, bool isArray, dynamic item)
        {
            if(t == typeof(string))
            {
                if (isArray)
                {
                    string[] arr = new string[item.Length];
                    for(int i = 0; i < item.Length; i++)
                    {
                        arr[i] = item[i];
                    }
                    ndw.PutArray(arr);
                }
                else
                    ndw.Put(item as string);
            }
            else if(t == typeof(int))
            {
                if (isArray)
                {
                    int[] arr = new int[item.Length];
                    for (int i = 0; i < item.Length; i++)
                    {
                        arr[i] = item[i];
                    }
                    ndw.PutArray(arr);
                }
                else
                    ndw.Put((int)item);
            }
            else if (t == typeof(bool))
            {
                if (isArray)
                {
                    bool[] arr = new bool[item.Length];
                    for (int i = 0; i < item.Length; i++)
                    {
                        arr[i] = item[i];
                    }
                    ndw.PutArray(arr);
                }
                else
                    ndw.Put((bool)item);
            }
            else if (t == typeof(double))
            {
                if (isArray)
                {
                    double[] arr = new double[item.Length];
                    for (int i = 0; i < item.Length; i++)
                    {
                        arr[i] = item[i];
                    }
                    ndw.PutArray(arr);
                }
                else
                    ndw.Put((double)item);
            }
            else if (t == typeof(float))
            {
                if (isArray)
                {
                    float[] arr = new float[item.Length];
                    for (int i = 0; i < item.Length; i++)
                    {
                        arr[i] = item[i];
                    }
                    ndw.PutArray(arr);
                }
                else
                    ndw.Put((float)item);
            }
        }


        //public static void PutFriendSlotPacket(this NetDataWriter ndw, FriendSlotPacket fsp)
        //{
        //    ndw.Put(fsp.Name); ndw.Put(fsp.Status); ndw.Put(fsp.AvatarId);
        //}
        //
        //public static void PutFriendSlotPackets(this NetDataWriter ndw, FriendSlotPacket[] fsp)
        //{
        //    ndw.PutArray(fsp.Select(friend => friend.Name).ToArray());
        //    ndw.PutArray(fsp.Select(friend => friend.Status).ToArray());
        //    ndw.PutArray(fsp.Select(friend => friend.AvatarId).ToArray());
        //}

        //static byte[] getBytes<T>(T str) where T : struct
        //{
        //    int size = Marshal.SizeOf(str);
        //    byte[] arr = new byte[size];
        //
        //    IntPtr ptr = Marshal.AllocHGlobal(size);
        //    Marshal.StructureToPtr(str, ptr, true);
        //    Marshal.Copy(ptr, arr, 0, size);
        //    Marshal.FreeHGlobal(ptr);
        //    return arr;
        //}
        //
        //public static void PutPacketsStruct<T>(this NetDataWriter ndw, T[] packets) where T : struct
        //{
        //    int sizetest = Marshal.SizeOf(packets[0]);
        //    byte[] put = new byte[sizetest * packets.Length];
        //    
        //    ndw.Put((ushort)packets.Length);
        //    ndw.Put((ushort)sizetest);
        //
        //    int pos = 0;
        //    foreach(T packet in packets)
        //    {
        //        foreach(byte b in getBytes(packet))
        //        {
        //            put[pos] = b;
        //            pos++;
        //        }
        //    }
        //
        //    ndw.Put(put);
        //    //foreach()
        //    //ndw.Put((ushort)packets.Length);
        //    //
        //    //ndw.Put((ushort)packets[0].Length);
        //    //ndw.Put(bytes);
        //}
        //
        //static byte[][] getBytesArray<T>(T[] str) where T : struct
        //{
        //    int size = Marshal.SizeOf(str);
        //    byte[][] total = new byte[str.Length][];
        //    for(int i = 0; i < str.Length; i++)
        //    {
        //        total[i] = new byte[size];
        //        total[i] = getBytes(str[i]);
        //    }
        //    return total;
        //}

        // static byte[] getBytes<T>(T[] str) where T : struct
        // {   
        //     int size = Marshal.SizeOf(str);
        //     byte[] arr = new byte[size];
        //
        //     IntPtr ptr = Marshal.AllocHGlobal(size);
        //     Marshal.StructureToPtr(str, ptr, true);
        //     Marshal.Copy(ptr, arr, 0, size);
        //     Marshal.FreeHGlobal(ptr);
        //     return arr;
        // }

    }
}