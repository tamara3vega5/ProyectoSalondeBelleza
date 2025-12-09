// =====================================
//   REGISTRO DE USUARIO NUEVO (texto plano)
// =====================================
document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("register-form");
  if (!form) return;

  form.addEventListener("submit", (e) => {
    e.preventDefault();

    const nombre = document.getElementById("reg-nombre").value.trim();
    const email  = document.getElementById("reg-email").value.trim();
    const pass   = document.getElementById("reg-pass").value.trim();

    let usuarios = JSON.parse(localStorage.getItem("usuarios")) || [];

    if (usuarios.find(u => u.email === email)) {
      mostrarMensaje("Este correo ya está registrado", true);
      return;
    }

    const nuevoUsuario = {
      nombre,
      email,
      password: pass,   // texto plano
      rol: "usuario"
    };

    usuarios.push(nuevoUsuario);
    localStorage.setItem("usuarios", JSON.stringify(usuarios));

    mostrarMensaje("Cuenta creada con éxito. Redirigiendo...");

    // Sesión unificada
    localStorage.setItem("usuarioConectado", JSON.stringify(nuevoUsuario));

    setTimeout(() => location.href = "index.html", 1200);
  });
});

function mostrarMensaje(texto, esError = false) {
  let msg = document.querySelector(".login-msg");
  if (!msg) {
    msg = document.createElement("p");
    msg.className = "login-msg";
    msg.style.marginTop = "12px";
    msg.style.fontWeight = "700";
    document.getElementById("register-form").appendChild(msg);
  }
  msg.style.color = esError ? "#d9534f" : "#5E925C";
  msg.textContent = texto;
}
