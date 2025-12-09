/* ===========================================================
   JS/initAdmin.js  (VERSIÓN FINAL)
   -----------------------------------------------------------
   - Fuente principal: localStorage["usuarios"]
   - Estructura válida: { nombre, email, password, rol }
   - Admin semilla garantizado:
        email: admin@matcha.com
        pass : 123456
        rol  : admin
   - Migración automática desde "users" → "usuarios" (si existía)
   - No usa console ni requiere interacción del usuario
   =========================================================== */

(function () {

  /* ===========================================================
     1) MIGRAR DESDE "users" (sistema viejo) → "usuarios"
     =========================================================== */
  try {
    const legacy = JSON.parse(localStorage.getItem("users"));

    if (Array.isArray(legacy) && legacy.length > 0) {

      // Convertir estructura vieja a la nueva
      const migrados = legacy.map(u => ({
        nombre:  u.nombre  || "Usuario",
        email:   u.email,
        password: u.password || u.pass || "",  // sin decodificar
        rol:     u.rol || "usuario",
      }));

      const actuales = JSON.parse(localStorage.getItem("usuarios")) || [];

      // Fusionar SIN duplicar emails (prioridad a "actuales")
      const mapa = new Map(actuales.map(u => [u.email, u]));

      for (const u of migrados) {
        if (!mapa.has(u.email)) mapa.set(u.email, u);
      }

      localStorage.setItem("usuarios", JSON.stringify(Array.from(mapa.values())));
      localStorage.removeItem("users"); // limpiar sistema viejo
    }
  } catch (e) {
    // Silencio para evitar errores en UI
  }


  /* ===========================================================
     2) GARANTIZAR QUE EXISTA EL ADMIN
     =========================================================== */
  let usuarios = JSON.parse(localStorage.getItem("usuarios")) || [];

  const existeAdmin = usuarios.some(u => u.email === "admin@matcha.com");

  if (!existeAdmin) {
    usuarios.push({
      nombre: "Administrador",
      email: "admin@matcha.com",
      password: "123456",
      rol: "admin",
    });

    localStorage.setItem("usuarios", JSON.stringify(usuarios));
  }

})();
