import React from 'react'

function LoginPage() {
  return (
    <div>
      <h1 className='text-blue-600'>Login Page!</h1>
      <form  className='flex'>
        
        <input type="email" placeholder='Email' />
        <input type="password" placeholder='Password' />
        <button type='submit'>submit</button>
      </form>
    </div>
  
    
  )
}

export default LoginPage
