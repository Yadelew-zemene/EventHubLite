"use client";

import { useState } from "react";
import { apiFetch } from "../lib/api";
import { useRouter } from "next/navigation";

 function RegisterForm() {
          const router = useRouter();
          const [error, setError] = useState("");

          async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
            e.preventDefault();

            const form = e.currentTarget;
            const body = {
              fullName: form.fullName.value,
              email: form.email.value,
              password: form.password.value,
            };

            try {
              const res = await apiFetch("/auth/register", {
                method: "POST",
                body: JSON.stringify(body),
              });

              localStorage.setItem("token", res.token);
              router.push("/dashboard");
            } catch (err: any) {
              setError(err.message);
            }
    };
  return (
    <form onSubmit={handleSubmit}>
      <input
        name="fullName"
        placeholder="Full name"
        required
      />
      <input
        name="email"
        type="email"
        placeholder="Email"
        required />
      <input
        name="password"
        type="password"
        placeholder="Password"
        required
        className=""
      />
      <button type="submit">Register</button>
      {error && <p>{error}</p>}
    </form>
  );
}
export default RegisterForm;
