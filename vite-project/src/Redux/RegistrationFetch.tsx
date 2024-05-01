import { registerRequest, registerSuccess, registerFailure } from './RegistrationSlice';

interface RegisterPayload {
    name: string;
    email: string;
    password: string;
    password2: string;
  }

export const register = (payload: RegisterPayload) => async (dispatch: any) => {
  dispatch(registerRequest());
  try {
    const response = await fetch('https://localhost:7287/UserRegistrationWeb', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });
    const result = await response.json();
    if (response.ok) {
      dispatch(registerSuccess());
    } else {
      dispatch(registerFailure(result.error || 'Registration failed'));
    }
  } catch (error) {
    dispatch(registerFailure('Registration failed'));
  }
};