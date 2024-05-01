import 'bootstrap/dist/css/bootstrap.min.css';
import { useState } from "react";
import { Form } from "react-bootstrap";

export function MyText()
{
    const [text, setText] = useState<string>("sdfasf");
    console.log(text);
    return (
        <Form.Control size="lg" type="text" placeholder="Large text" onChange={(e)=>setText(e.target.value)}/>
    );
}