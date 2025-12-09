const API_CLIENTES = "https://localhost:7024/api/Clientes";

// -------------------------------------------
// FUNCIÓN PARA REGISTRO
// -------------------------------------------
document.getElementById("registerForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const nombre = document.getElementById("nombre").value.trim();
    const correo = document.getElementById("correo").value.trim();
    const telefono = document.getElementById("telefono").value.trim();
    const password = document.getElementById("password").value.trim();

    if (!nombre || !correo || !password) {
        alert("Por favor completa los campos obligatorios.");
        return;
    }

    // Objeto que coincide EXACTAMENTE con ClienteCreateDto.cs
    const nuevoCliente = {
        nombre: nombre,
        correo: correo,
        telefono: telefono,
        password: password,
        fechaRegistro: new Date().toISOString()
    };

    try {
        const res = await fetch(API_CLIENTES, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(nuevoCliente)
        });

        if (!res.ok) {
            alert("No se pudo registrar el cliente.");
            return;
        }

        const cliente = await res.json();

        // Guardar sesión automáticamente
        localStorage.setItem("usuario", JSON.stringify({
            idCliente: cliente.idCliente,
            nombre: cliente.nombre,
            correo: cliente.correo,
            rol: cliente.rol || "cliente"
        }));

        alert("Registro exitoso, bienvenido " + cliente.nombre);
        window.location.href = "index.html";

    } catch (error) {
        console.error("Error al registrar:", error);
        alert("Error al conectar con el servidor.");
    }
});
