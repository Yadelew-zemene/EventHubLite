"use client"

import React from 'react'
import { useState } from 'react';
import { apiFetch } from '../lib/api';
import { useRouter } from 'next/navigation';
const LoginForm = () =>
{
    const router = useRouter();
    const [error, setError] = useState("");
    async function handleSubmit(e: React.FormEvent<HTMLFormElement>)
           {
                    e.preventDefault();
                    const form = e.currentTarget;
                    const body =
                                {
                                    email: form.email.value,
                                    password:form.password.value
                                }
                    try {
                            const res = await apiFetch("/auth/login",
                                        {
                                        method: "POST",
                                        body: JSON.stringify(body)
                                    });
                           document.cookie = `token=${res.token}; path=/`;

                            router.push("/dashboard");

                } 
                    catch (error: any) {
                        setError(error.message);
                    }
        
         }
return (
      <form onSubmit={handleSubmit} className=''>
        <input
            
            name='email'
            type="email"
            placeholder='Email'
            required
            className=' flex-3 border-4 border-amber-950'
        />
        <input
            name='password'
            type="password"
            placeholder='Password'
            required
            className='flex-3 border-4 border-amber-950'
        />
        <button
            type='submit'
            className='border-2 border-amber-950 text-white bg-blue-600'
        >Login</button>
        {error && <p>{error}</p>}
          
     </form>
      
  )
}

export default LoginForm