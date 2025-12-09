const API_CLIENTES = "https://localhost:7024/api/Clientes";

/* ===========================================================
   REGISTRO DE NUEVO CLIENTE
   =========================================================== */
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

    // OBJETO EXACTO PARA ClienteCreateDto (tu backend)
    const nuevoCliente = {
        nombre: nombre,
        correo: correo,
        telefono: telefono,
        password: password   // El backend genera el hash
    };

    try {
        const res = await fetch(API_CLIENTES, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(nuevoCliente)
        });

        // -------------------------------
        // ERRORES DEL BACKEND (correo duplicado, etc.)
        // -------------------------------
        if (!res.ok) {
            const errorData = await res.json().catch(() => null);

            if (errorData?.mensaje) {
                alert(errorData.mensaje); // Mensaje del backend
            } else {
                alert("No se pudo registrar el cliente.");
            }
            return;
        }

        const cliente = await res.json();

        // -------------------------------
        // GUARDAR SESIÓN AUTOMÁTICAMENTE
        // -------------------------------
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
