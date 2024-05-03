import { Button, FloatingLabel, Form } from "react-bootstrap";
import { useAppDispatch } from '../Redux/Hooks';
import { register } from "../Redux/RegistrationFetch";
import { useState } from "react";

export function ButtonReg() {
  const dispatch = useAppDispatch();
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [password2, setPassword2] = useState("");

  const handleClick = () => {
    dispatch(
      register({
        name,
        email,
        password,
        password2,
      }, email, password)
    );
  };

  return (
    <>
      <FloatingLabel
        controlId="floatingInput"
        label="Name"
        className="mb-3"
      >
        <Form.Control
          type="text"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </FloatingLabel>
      <FloatingLabel
        controlId="floatingInput"
        label="Email address"
        className="mb-3"
      >
        <Form.Control
          type="email"
          placeholder="name@example.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </FloatingLabel>
      <FloatingLabel controlId="floatingPassword" label="Password">
        <Form.Control
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </FloatingLabel>
      <FloatingLabel controlId="floatingPassword2" label="Confirm Password">
        <Form.Control
          type="password"
          placeholder="Confirm Password"
          value={password2}
          onChange={(e) => setPassword2(e.target.value)}
        />
      </FloatingLabel>

      <Button variant="primary" onClick={handleClick}>
        Register
      </Button>
    </>
  );
}