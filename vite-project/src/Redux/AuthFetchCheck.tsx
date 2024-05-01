import { CheckRequest, CheckFailure, CheckSuccess } from "./AuthSliceCheck";


export const check = () => async (dispatch: any) => {
    
  dispatch(CheckRequest());
  try {
    console.log(`Bearer ${localStorage.getItem("accessToken")}`);
    const response = await fetch('https://localhost:7287/CheckWeb', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`
      },
    });
    const result = await response.json();
    if (response.ok) {
      dispatch(CheckSuccess());
      console.log(3);
    } else {
      dispatch(CheckFailure(result.error || 'Registration failed'));
    }
  } catch (error) {
    
  }
};