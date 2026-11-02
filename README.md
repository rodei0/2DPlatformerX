# Platformer2D
Vertical slice de un platformer 2D hecho en Unity, centrado en el game feel y la precisión del control.

![gameplay](docs/GameplayGif.gif)

## Features

**Movimiento**
- Nuevo input system de Unity utilizado
- Dash en todas las direcciones
- Altura de salto variable, coyote time y input buffer
- Objetos que restauran el dash y el salto

**Game feel**
- Squash & stretch en saltos y aterrizajes
- Screen shake
- Partículas

**Mecánicas de nivel**
- Plataformas móviles
- Proyectiles en movimiento
- Cambio de elemento que altera como se interactúa con el entorno
- Sistema de botones y puertas
- Sistema de checkpoints

**Sistemas**
- Sistema de guardado en JSON que gestiona los checkpoints y la asignación de teclas principalmente
- Reasignación de teclas desde las opciones
- Audio implementado con FMOD

![rebind](docs/RebindSettingsGif.gif)

## Mi rol
Programación, arte y diseño (proyecto en solitario).

## Scripts relevantes
- [`PlayerMovement.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/Player/PlayerMovement.cs): movimiento, salto variable, coyote time e input buffer
- [`PlayerDash.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/Player/PlayerDash.cs): dash en todas direcciones
- [`PlayerVFX.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/Player/PlayerVFX.cs): squash & stretch y partículas
- [`SaveSystem.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/SaveSystem/SaveSystem.cs), [`SaveData.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/SaveSystem/SaveData.cs) y [`SaveManager.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/Managers/SaveManager.cs): guardado en JSON
- [`AudioManager.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/Managers/AudioManager.cs) y [`FMODEvents.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/Scripts/Managers/FMODEvents.cs): integración con FMOD
- [`RebindScript.cs`](https://github.com/rodei0/2DPlatformerX/blob/main/Assets/InputSystem/RebindScript.cs): sistema de reasignación de teclas

## Jugar
https://rodei.itch.io/platformerverticalslice

## Créditos
SFX: Pixabay
