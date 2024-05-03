import { auth } from './AuthFetch';
import { registerRequest, registerSuccess, registerFailure } from './RegistrationSlice';

interface RegisterPayload {
    name: string;
    email: string;
    password: string;
    password2: string;
  }

export const register = (payload: RegisterPayload, email: string, password : string) => async (dispatch: any) => {
  dispatch(registerRequest());
  try {
    const response = await fetch('https://localhost:7287/UserRegistrationWeb', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });
    console.log("1");
    const result = await response;
    if (response.ok) {
      console.log("2");
      dispatch(auth({
        email,
        password,
      }))
      console.log("3");
      dispatch(registerSuccess());
    } else {
      dispatch(registerFailure('Registration failed'));
    }
  } catch (error) {
    dispatch(registerFailure('Registration failed'));
  }
};