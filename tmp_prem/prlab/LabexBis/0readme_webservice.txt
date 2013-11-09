
Spremembe potrebne pri Web Service proxy objektu (reference.cs):

Pri razredu, ki predstavlja web service:
  Atribute je treba nastaviti tako:
      [System.Diagnostics.DebuggerStepThroughAttribute()]
      [System.ComponentModel.DesignerCategoryAttribute("code")]
      [System.Web.Services.WebServiceBindingAttribute(Name = "calypsoNSSoapBinding", Namespace = "http://webservice.calypso.infonet.com")]
      [System.Xml.Serialization.XmlIncludeAttribute(typeof(CalypsoException))]

Predvsem je važen WebServiceBindingAttribute, kjer je treba popraviti Namespace!!!


Pri metodah (receiveMessage, ):
  Atribute je treba nastaviti tako:
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://webservice.calypso.infonet.com", ResponseNamespace = "http://webservice.calypso.infonet.com", Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        [return: System.Xml.Serialization.XmlElementAttribute("receiveMessageReturn")]

Pri atributu SoapRpcMethodAttribute je treba popraviti ResponseNamespace, ker .NET pretvori ime domene
v IP naslov (zakaj to naredi?)


Pri tipih (public partial class Message):
  Zakomentirati vrstico 
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="valueobjects.calypso.infonet.com")]
  Tako morajo biti atributi pri tipu naslednji:
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]

  
  Pri posameznih property-jih je potrebno uporabiti
     Form = System.Xml.Schema.XmlSchemaForm.Qualified
   namesto Unqualified. Tako morajo biti pri lastnostih atributi:

        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, IsNullable=true)]



***********************************************

          IZVEDBA PROCEDURE - POPRAVKI

***********************************************

1. Pri class CalypsoWSNSService :  
atribut 
    [System.Web.Services.WebServiceBindingAttribute(Name = "calypsoNSSoapBinding", Namespace = "http://webservice.calypso.infonet.com")]
(popravljen Namespace=...)

2. Pri vseh metodah: 
v atributu popravljen atribut 
    ResponseNamespace="http://192.168.0.21:8080/InfonetWS/services/calypsoNS"
v
    RequestNamespace = "http://webservice.calypso.infonet.com"
(7 zamenjav)


V TEJ FAZI DELA getUniqueID. 

3. Pri definiciji tipa Message:
zakomentiran je atribut:
      [System.Xml.Serialization.XmlTypeAttribute(Namespace="valueobjects.calypso.infonet.com")]


4. Pri definiciji tipa Message, pri obeh lastnostih (text in ID):
	v atributu spremeniš Unqualified v Qualified  (po Goranovem primeru sodeè dela tudi, èe cel atribut zakomentiraš)

ZADEVA ŠE VEDNO NE DELA!



********* IZVEDBA PROCEDURE - POPRAVKI ********
***********************************************

