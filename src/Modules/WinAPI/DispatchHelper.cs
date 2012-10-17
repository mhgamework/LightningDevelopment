using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Modules
{
    public class DispatchHelper
    {
        public static TypeInformation[] GetTypes(object com_obj)
        {
            var obj = (IDispatch)com_obj;
            int count;
            obj.GetTypeInfoCount(out count);


            // MSDN says count is 0 or 1 :P
            var ret = new TypeInformation[count];

            for (int i = 0; i < count; i++)
            {
                ITypeInfo typeInfo;
                obj.GetTypeInfo(0, 0, out typeInfo);
                string strName, strDocString, strHelpFile;
                int dwHelpContext;
                typeInfo.GetDocumentation(-1, out strName, out strDocString, out dwHelpContext, out strHelpFile);
                ret[i] = new TypeInformation
                             {
                                 Name = strName,
                                 Documentation = strDocString,
                                 HelpContext = dwHelpContext,
                                 HelpFile = strHelpFile
                             };
            }

            return ret;


        }

        // This static function returns an array containing
        // information of all public methods of a COM object.
        // Note that the COM object must implement the IDispatch
        // interface in order to be usable in this function.
        public static MethodInformation[] GetMethodInformation(object com_obj)
        {
            IDispatch pDispatch = null;

            try
            {
                // Obtain the COM IDispatch interface from the input com_obj object.
                // Low-level-wise this causes a QueryInterface() to be performed on
                // com_obj to obtain the IDispatch interface from it.
                pDispatch = (IDispatch)com_obj;
            }
            catch (System.InvalidCastException ex)
            {
                // This means that the input com_obj
                // does not support the IDispatch
                // interface.
                return null;
            }

            // Obtain the ITypeInfo interface from the object via its
            // IDispatch.GetTypeInfo() method.
            System.Runtime.InteropServices.ComTypes.ITypeInfo pTypeInfo = null;
            // Call the IDispatch.GetTypeInfo() to obtain an ITypeInfo interface
            // pointer from the com_obj.
            // Note that the first parameter must be 0 in order to
            // obtain the Type Info of the current object.
            pDispatch.GetTypeInfo
                (
                    0,
                    0,
                    out pTypeInfo
                );

            // If for some reason we are not able to obtain the
            // ITypeInfo interface from the IDispatch interface
            // of the COM object, we return immediately.
            if (pTypeInfo == null)
            {
                return null;
            }

            // Get the TYPEATTR (type attributes) of the object
            // via its ITypeInfo interface.
            IntPtr pTypeAttr = IntPtr.Zero;
            System.Runtime.InteropServices.ComTypes.TYPEATTR typeattr;
            // The TYPEATTR is returned via an IntPtr.
            pTypeInfo.GetTypeAttr(out pTypeAttr);
            // We must convert the IntPtr into the TYPEATTR structure
            // defined in the System.Runtime.InteropServices.ComTypes
            // namespace.
            typeattr = (System.Runtime.InteropServices.ComTypes.TYPEATTR)Marshal.PtrToStructure
                                                                             (
                                                                                 pTypeAttr,
                                                                                 typeof(System.Runtime.InteropServices.ComTypes.TYPEATTR)
                                                                             );
            // Release the resources related to the obtaining of the
            // COM TYPEATTR structure from an ITypeInfo interface.
            // From now onwards, we will only work with the
            // System.Runtime.InteropServices.ComTypes.TYPEATTR
            // structure.
            pTypeInfo.ReleaseTypeAttr(pTypeAttr);
            pTypeAttr = IntPtr.Zero;

            // The TYPEATTR.guid member indicates the default interface implemented
            // by the COM object.
            Guid defaultInterfaceGuid = typeattr.guid;

            MethodInformation[] method_information_array = new MethodInformation[typeattr.cFuncs];
            // The TYPEATTR.cFuncs member indicates the total number of methods
            // that the current COM object implements.
            for (int i = 0; i < (typeattr.cFuncs); i++)
            {
                // We loop through the number of methods.
                System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc;
                IntPtr pFuncDesc = IntPtr.Zero;
                string strName;
                string strDocumentation;
                int iHelpContext;
                string strHelpFile;

                // During each loop, we use the ITypeInfo.GetFuncDesc()
                // method to obtain a FUNCDESC structure which describes
                // the current method indexed by "i".
                pTypeInfo.GetFuncDesc(i, out pFuncDesc);
                // The FUNCDESC structure is returned as an IntPtr.
                // We need to convert it into a FUNCDESC structure
                // defined in the System.Runtime.InteropServices.ComTypes
                // namespace.
                funcdesc = (System.Runtime.InteropServices.ComTypes.FUNCDESC)Marshal.PtrToStructure
                                                                                 (
                                                                                     pFuncDesc,
                                                                                     typeof(System.Runtime.InteropServices.ComTypes.FUNCDESC)
                                                                                 );
                // The FUNCDESC.memid contains the member id of the current function
                // in the Type Info of the object.
                // Use the ITypeInfo.GetDocumentation() with reference to memid
                // to obtain information for this function.
                pTypeInfo.GetDocumentation
                    (
                        funcdesc.memid,
                        out strName,
                        out strDocumentation,
                        out iHelpContext,
                        out strHelpFile
                    );
                // Fill up the appropriate method_information_array element
                // with field information.
                method_information_array[i].m_strName = strName;
                method_information_array[i].m_strDocumentation = strDocumentation;

                if (funcdesc.invkind == System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC)
                {
                    method_information_array[i].m_method_type = MethodType.Method;
                }
                else if (funcdesc.invkind == System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET)
                {
                    method_information_array[i].m_method_type = MethodType.Property_Getter;
                }
                else if (funcdesc.invkind == System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT)
                {
                    method_information_array[i].m_method_type = MethodType.Property_Putter;
                }
                else if (funcdesc.invkind == System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF)
                {
                    method_information_array[i].m_method_type = MethodType.Property_PutRef;
                }

                // The ITypeInfo.ReleaseFuncDesc() must be called
                // to release the (unmanaged) memory of the FUNCDESC
                // returned in pFuncDesc (an IntPtr).
                pTypeInfo.ReleaseFuncDesc(pFuncDesc);
                pFuncDesc = IntPtr.Zero;
            }

            return method_information_array;
        }

        public enum MethodType
        {
            Method = 0,
            Property_Getter = 1,
            Property_Putter = 2,
            Property_PutRef = 3
        }

        public struct MethodInformation
        {
            public string m_strName;
            public string m_strDocumentation;
            public MethodType m_method_type;
        }

        public class TypeInformation
        {
            public string Name;
            public string Documentation;
            public int HelpContext;
            public string HelpFile;

            public override string ToString()
            {
                return "COMInterface: " + Name;
            }
        }
    }
}