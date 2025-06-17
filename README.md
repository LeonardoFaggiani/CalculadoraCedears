﻿## Proposito del proyecto

La idea de este proyecto es generar una calculadora de cedears para que se pueda calcular la ganancias de uno o varios cedears sin tener en cuenta la fluctuación del tipo de cambio del dólar americano (CCL).

## Que resuelve `CalculadoraCedears`

Como en varios brokers Argentinos no hay forma de calcular la ganancia verdadera, si no que se ve "afectada" por el tipo de cambio del dolar, por este motivo surgio la idea de crear esta calculadora para poder obtener de una manera rapida y facil la ganancia neta.

### 🔧 Configuración

#### Este proyecto requiere que configures tus propias credenciales de Google para poder autenticar usuarios.
Para ello, haz lo siguiente:

Ve a  `src-tauri/oauth_config.json` y reemplazá los valores:
   - `$GOOGLE_CLIENT_ID` por tu `ClientId` de Google.
   - `$GOOGLE_CLIENT_SECRET` por tu  `ClientSecret` de Google.

Tambien se debe modificar en la API ya que se utiliza `ClientId` y demas opciones de JWT para validar el `AccessToken` generado por el login de Google.
En `appsettings.Development.json` reemplazar los valores:
   - `$GOOGLE_CLIENT_ID` por tu `ClientId` de Google.
   - `$JWT_SECRET` por tu clave, si estas en Windows poder usar el siguiente comando `openssl rand -base64 32` para generar una clave.
   - `$JWT_ISSUER` por tu Issuer
   - `$JWT_AUDIENCE` por tu Audience

> [!NOTE]
> Es consejable guardar tus claves de Google y JWT en los secrets de GitHub y luego utilizarlos en GitHub Actions.


## Iniciar Aplicación 
  1. Ir al proyecto de base de datos que sen encuentra en la API `CalculadoraCedears/CalculadoraCedears.Api.Db` y publicar, se necesita SQLExpress instalado para poder deployarla en un localhost.
  2. Levantar la API que se encuentra en `CalculadoraCedears/CalculadoraCedears.Api` y ejecutar el comando `dotnet run` esto levantara la API en `https://localhost:7016`
  3. Como ultimo, la aplicacion `Tauri`, debemos ir a la carpeta `CalculadoraCedears/CalculadoraCedears.Desktop` y ejecutar el comando `npm run tauri` esto levanta la App Desktop apuntando a la API `https://localhost:7016`

> [!NOTE]
> El scaffolding del proyecto fue genero completamente por Custom Api Template para proyectos NET Core.
> ```bash
> dotnet new install Custom.Api.Template::1.0.2
> ```
> para mayor informacion ℹ️ [CustomApiTemplate](https://github.com/LeonardoFaggiani/Template)


## Dame una Estrellita! ⭐
Si te gusta lo que hago o te sirvio `Calculadora Cedears` podes darme una ⭐ como muestra de soporte 😉