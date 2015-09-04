﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModuleInject {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ModuleInject.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Interception was not activated on the module with type &apos;{0}&apos;. Activate interception in the module before adding a behaviour to components..
        /// </summary>
        internal static string ComponentRegistrationContext_InterceptionNotActivated {
            get {
                return ResourceManager.GetString("ComponentRegistrationContext_InterceptionNotActivated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The component of name &apos;{0}&apos; and type &apos;{1}&apos; in module &apos;{2}&apos; has no dispose strategy defined despite it being derived from IDisposable..
        /// </summary>
        internal static string Construction_NoDisposeStrategySetForDisposableType {
            get {
                return ResourceManager.GetString("Construction_NoDisposeStrategySetForDisposableType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; of type &apos;{1}&apos; has no setter defined..
        /// </summary>
        internal static string DependencyInjectionContext_IntoPropertyExpressionNotWritable {
            get {
                return ResourceManager.GetString("DependencyInjectionContext_IntoPropertyExpressionNotWritable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The target given to the IntoProperty method does not describe a property..
        /// </summary>
        internal static string DependencyInjectionContext_NoMemberAccessInTargetExpression {
            get {
                return ResourceManager.GetString("DependencyInjectionContext_NoMemberAccessInTargetExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The component with name &apos;{0}&apos; and type &apos;{1}&apos; is already registered in the injection container..
        /// </summary>
        internal static string InjectionContainer_ComponentAlreadyRegistered {
            get {
                return ResourceManager.GetString("InjectionContainer_ComponentAlreadyRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The component with name &apos;{0}&apos; and type &apos;{1}&apos; is not registered in the injection container..
        /// </summary>
        internal static string InjectionContainer_ComponentNotRegistered {
            get {
                return ResourceManager.GetString("InjectionContainer_ComponentNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The module with type &apos;{0}&apos; was already resolved. Please avoid multiple resolutions of the same module..
        /// </summary>
        internal static string InjectionModule_AlreadyResolved {
            get {
                return ResourceManager.GetString("InjectionModule_AlreadyResolved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property or method &apos;{0}&apos; of module with type &apos;{1}&apos; could not be registered. Only direct properties and methods of a module can be registered..
        /// </summary>
        internal static string InjectionModule_CannotRegisterPropertyOrMethodsWhichAreNotMembersOfTheModule {
            get {
                return ResourceManager.GetString("InjectionModule_CannotRegisterPropertyOrMethodsWhichAreNotMembersOfTheModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The component &apos;{0}&apos; of module with type &apos;{1}&apos; is not registered. Could not create instance of requested component..
        /// </summary>
        internal static string InjectionModule_ComponentNotRegistered {
            get {
                return ResourceManager.GetString("InjectionModule_ComponentNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The component &apos;{0}&apos; of module with type &apos;{1}&apos; was tried to be resolved before the module was resolved. Please resolve the module before creating any instances..
        /// </summary>
        internal static string InjectionModule_CreateInstanceBeforeResolve {
            get {
                return ResourceManager.GetString("InjectionModule_CreateInstanceBeforeResolve", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The method &apos;{0}&apos; of module with type &apos;{1}&apos; can not be registered as a factory method. Only parameterless methods can currently be registered..
        /// </summary>
        internal static string InjectionModule_FactoryMethodsWithParametersNotSupportedYet {
            get {
                return ResourceManager.GetString("InjectionModule_FactoryMethodsWithParametersNotSupportedYet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The module with type &apos;{0}&apos; has invalid properties that are neither private nor public properties. Declare them as public via the module interface or apply the PrivateComponent attribute. 
        ///The properties are: &apos;{1}&apos;.
        /// </summary>
        internal static string InjectionModule_InvalidProperty {
            get {
                return ResourceManager.GetString("InjectionModule_InvalidProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The method &apos;{0}&apos; of module with type &apos;{1}&apos; is not qualified to be a private factory. Either it is a public factory or it was not marked with the PrivateFactoryAttribute..
        /// </summary>
        internal static string InjectionModule_MethodNotQualifiedForPrivateRegistration {
            get {
                return ResourceManager.GetString("InjectionModule_MethodNotQualifiedForPrivateRegistration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The module with type &apos;{0}&apos; does not possess an interface. Please provide one in the generic parameters of InjectionModule and implement it in your module..
        /// </summary>
        internal static string InjectionModule_ModulesMustHaveAnInterface {
            get {
                return ResourceManager.GetString("InjectionModule_ModulesMustHaveAnInterface", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Multiple registrations were found for the type &apos;{0}&apos; in module of type &apos;{1}&apos;. Therefore, you can not get a component of that interface without its name..
        /// </summary>
        internal static string InjectionModule_MultipleRegistrationsUnderInterface {
            get {
                return ResourceManager.GetString("InjectionModule_MultipleRegistrationsUnderInterface", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expression &apos;{1}&apos; describes neither a property or method of the module with type &apos;{0}&apos;..
        /// </summary>
        internal static string InjectionModule_NeitherPropertyNorMethodExpression {
            get {
                return ResourceManager.GetString("InjectionModule_NeitherPropertyNorMethodExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; of module with type &apos;{1}&apos; is not qualified to be a private component. Either it is a public component or it was not marked with the PrivateComponentAttribute..
        /// </summary>
        internal static string InjectionModule_PropertyNotQualifiedForPrivateRegistration {
            get {
                return ResourceManager.GetString("InjectionModule_PropertyNotQualifiedForPrivateRegistration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The types &apos;{0}&apos;, &apos;{1}&apos; of the injection register do not match the given types &apos;{2}&apos;, &apos;{3}&apos;..
        /// </summary>
        internal static string InjectionRegister_TypeMismatch {
            get {
                return ResourceManager.GetString("InjectionRegister_TypeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expression &apos;{0}&apos; is not a continuous chain of member accesses..
        /// </summary>
        internal static string MemberChainEvaluator_MemberChainNotContinuous {
            get {
                return ResourceManager.GetString("MemberChainEvaluator_MemberChainNotContinuous", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expression &apos;{0}&apos; has not the correct root type..
        /// </summary>
        internal static string MemberChainEvaluator_RootTypeMismatch {
            get {
                return ResourceManager.GetString("MemberChainEvaluator_RootTypeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expression &apos;{0}&apos; is currently not supported as a method call argument..
        /// </summary>
        internal static string MethodCallArgumentNotSupported {
            get {
                return ResourceManager.GetString("MethodCallArgumentNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The registration hook of type &apos;{1}&apos; can not be added to the module of type &apos;{0}&apos; because it does not apply to it..
        /// </summary>
        internal static string Module_RegistrationHookDoesNotApply {
            get {
                return ResourceManager.GetString("Module_RegistrationHookDoesNotApply", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; of module with type &apos;{1}&apos; could not be resolved. Neither the module nor the registry of the module contained the component. Did you inject a property that was neither a member of the modules interface nor marked with the PrivateComponentAttribute? Did you forget to put the component into the registry or setting the registry of the module?.
        /// </summary>
        internal static string ModuleResolver_MissingPropertyRegistration {
            get {
                return ResourceManager.GetString("ModuleResolver_MissingPropertyRegistration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; of module with type &apos;{1}&apos; has no interface. Please make sure that all properties of your modules are implemented with interface types..
        /// </summary>
        internal static string ModuleResolver_PropertyIsNoInterface {
            get {
                return ResourceManager.GetString("ModuleResolver_PropertyIsNoInterface", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; of module with type &apos;{1}&apos; is already set before resolving it despite not being marked with ExternalComponentAttribute. Please set the ExternalComponentAttribute if the property is set outside the resolution process of the module itself..
        /// </summary>
        internal static string ModuleResolver_PropertyWithoutExternalAttribute {
            get {
                return ResourceManager.GetString("ModuleResolver_PropertyWithoutExternalAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The constructor of component &apos;{0}&apos; in module of type &apos;{1}&apos; was already injected before..
        /// </summary>
        internal static string RegistrationContext_ConstructorAlreadyCalled {
            get {
                return ResourceManager.GetString("RegistrationContext_ConstructorAlreadyCalled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The registry is already composed, you can not add further catalogs to it..
        /// </summary>
        internal static string Registry_AlreadyComposedNoFurtherCatalogs {
            get {
                return ResourceManager.GetString("Registry_AlreadyComposedNoFurtherCatalogs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The registry does not contain a registration for the type &apos;{0}&apos;..
        /// </summary>
        internal static string RegistryModule_TypeNotRegistered {
            get {
                return ResourceManager.GetString("RegistryModule_TypeNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The instance with key &apos;{0}&apos; of type &apos;{1}&apos; could no be found by the service locator..
        /// </summary>
        internal static string ServiceLocator_CouldNotFindInstanceRegistration {
            get {
                return ResourceManager.GetString("ServiceLocator_CouldNotFindInstanceRegistration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are multiple registrations of type &apos;{1}&apos; in the service locator..
        /// </summary>
        internal static string ServiceLocator_MultipleRegistrationsFound {
            get {
                return ResourceManager.GetString("ServiceLocator_MultipleRegistrationsFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A service of type &apos;{0}&apos; is already registered in this service provider..
        /// </summary>
        internal static string ServiceProvider_AlreadyContainsThisServiceType {
            get {
                return ResourceManager.GetString("ServiceProvider_AlreadyContainsThisServiceType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A service of type &apos;{0}&apos; is not registered in this service provider..
        /// </summary>
        internal static string ServiceProvider_DoesNotContainThisServiceType {
            get {
                return ResourceManager.GetString("ServiceProvider_DoesNotContainThisServiceType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; of type  &apos;{1}&apos; does not have a setter anywhere in the module inheritance chain..
        /// </summary>
        internal static string TypeExtensions_NoPropertySetterFound {
            get {
                return ResourceManager.GetString("TypeExtensions_NoPropertySetterFound", resourceCulture);
            }
        }
    }
}
