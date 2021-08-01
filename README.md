# Prueba técnica para Mercado Libre por Alexander Cubillos Jauregui

## Proyecto y Estructura del mismo

Se usan tecnologías Microsoft básicamente.
La solución se construyó usando NET CORE 3.1.
El proyecto está divido en 3 partes.


![project](/resources/img-001.png)

La primera parte es el proyecto "Core" el cual tiene toda la lógica base del proyecto. Contiene los Dominios, Proveedores de datos, Acceso a Datos, Entidades y Modelos. Hace uso de los paquetes Nugets para trabajar con **Entity Framework** y **Bases de datos en memoria**.

La segunda parte es el proyecto "Unit Test", el cual prueba el código contenido en el proyecto "Core". Hace uso de los paquetes Nugets de **Xunit** y **Moq** para crear las pruebas unitarias.

La tercera y última parte es el proyecto "Web", el cual hace uso de la lógica contenida en el proyecto "Core" y expone los diferentes endpoints solicitados en la prueba técnica entregada.

***

## Cómo ejecutar el programa

En primer lugar, descargue e instale el [SDK de .NET](https://dotnet.microsoft.com/download/dotnet) en el equipo.

A continuación, abra un terminal como PowerShell, Símbolo del sistema o bash y ubíquese en el directorio base del repositorio de código. ej: __'C:\repos\alexjaur\mercadolibre-technicalproof\ '__. Luego escriba el comando `dotnet` siguiente:

```
 dotnet run --project source/MercadoLibre.TechnicalProof.ByAl3xJauregui.WebApp
```

Verá la salida siguiente:
```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\repos\alexjaur\mercadolibre-technicalproof\source\MercadoLibre.TechnicalProof.ByAl3xJauregui.WebApp
```

Ahora, podra hacer los diferentes llamados tal cual fueron espeficicados en el documento "[Challenge Técnico.pdf](/resources/Challenge-Técnico.pdf)" teniendo como URL base local `https://localhost:5001/`

También lo pueden ejecutar desde Visual Studio. Solo sería necesario tener instalado en SDK y ejecutar la aplicación web con **F5**. Asimismo, desde Visual Studio, podrá ingresar a las pruebas unitarias y ejecutarlas de forma individual o grupal.

***

## Version publicada online

La versión online está hospedada en **Azure**.
La URL base es: [https://al3x-jauregui-mercado-libre-technical-proof.azurewebsites.net](https://al3x-jauregui-mercado-libre-technical-proof.azurewebsites.net)

***

## Buenas prácticas

El proyecto muestra la implementación de varias buenas prácticas. Haciendo uso de la adecuada nomenclatura, espaciado, código auto-descriptivo, limpio, conceptos como inyección de dependencias, arquitectura orientada a servicios/dominios, principios S.O.L.I.D, pruebas unitarias, cubrimiento de código, entre otros.

***

## Postman

Adicionalmente, cree y guarde los archivos usados con Postman, los cuales podrán importar en la aplicación Postman. Están en la carpeta `~/postman/`.

***

## Recursos

- [SDK de .NET](https://dotnet.microsoft.com/download/dotnet)

 - [Instalación de .NET en Windows](https://docs.microsoft.com/es-es/dotnet/core/install/windows?tabs=net50)
