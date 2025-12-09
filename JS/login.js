const API = "https://localhost:7024/api/Auth/login";

// ---------------------------
// CAPTURAR BOTÓN DE LOGIN
// ---------------------------
document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const correo = document.getElementById("correo").value.trim();
    const password = document.getElementById("password").value.trim();

    if (!correo || !password) {
        alert("Por favor completa todos los campos.");
        return;
    }

    try {
        const res = await fetch(API, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ correo, password })
        });

        if (!res.ok) {
            alert("Correo o contraseña incorrectos.");
            return;
        }

        const data = await res.json();
        const cliente = data.cliente;

        // ---------------------------
        // GUARDAR SESIÓN EN LOCALSTORAGE
        // ---------------------------
        localStorage.setItem("usuario", JSON.stringify({
            idCliente: cliente.idCliente,
            nombre: cliente.nombre,
            correo: cliente.correo,
            rol: cliente.rol || "cliente"
        }));

        // ---------------------------
        // REDIRECCIONAR SEGÚN ROL
        // ---------------------------
        if (cliente.rol === "admin") {
            window.location.href = "admin.html";
        } else {
            window.location.href = "index.html";
        }

    } catch (error) {
        console.error("Error en login:", error);
        alert("Error al conectar con el servidor.");
    }
});
