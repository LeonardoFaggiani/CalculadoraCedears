## Proposito del proyecto

La idea de este proyecto es generar una calculadora de cedears para que se pueda calcular la ganancias de uno o varios cedears sin tener en cuenta la fluctuación del tipo de cambio del dólar americano (CCL).

## Que resuelve `CalculadoraCedears`

Como en varios brokers Argentinos no hay forma de calcular la ganancia verdadera, si no que se ve "afectada" por el tipo de cambio del dolar, por este motivo surgio la idea de crear esta calculadora para poder obtener de una manera rapida y facil la ganancia neta.

### 🔧 Configuración

#### Este proyecto requiere que configures tus propias credenciales de Google para poder autenticar usuarios.
Para ello, seguí estos pasos:

1. Ve a  `src-tauri/oauth_config.json` y reemplazá los valores:
   - `$GOOGLE_CLIENT_ID`
   - `$GOOGLE_CLIENT_SECRET`
por tu `ClientId` y `ClientSecret` de Google.
> Es consejable guardar tus claves de Google en los secrets de GitHub y luego utilizarlos en GitHub Actions.
<br>


> [!NOTE]
> El scaffolding del proyecto fue genero completamente por Custom Api Template para proyectos NET Core.
> ```bash
> dotnet new install Custom.Api.Template::1.0.2
> ```
> para mayor informacion ℹ️ [CustomApiTemplate](https://github.com/LeonardoFaggiani/Template)


## Dame una Estrellita! ⭐
Si te gusta lo que hago o te sirvio `Calculadora Cedears` podes darme una ⭐ como muestra de soporte 😉
