import React from "react";
import { Col, Container, Row } from "react-bootstrap";
import Image from "react-bootstrap/Image";
import "../css/AboutPage.css"; // Импортируем файл стилей

export function AboutPage() {
    return (
        <Container>
            <div className="text-container">
                <p>Добро пожаловать в интернет-магазин "Paper Street Soap"!</p>
                <p>Мы являемся небольшой компанией, занимающейся производством высококачественного мыла ручной работы.</p>
                <p>Мы верим, что в мире существует немало неприятностей, которые можно решить просто хорошим мылом. Мы создаем наше мыло с большой любовью и заботой о качестве каждого ингредиента, чтобы вы получили настоящее удовольствие от использования нашей продукции. </p>
                <p>В нашем магазине вы найдете широкий выбор мыла, включая классические ароматы и уникальные композиции, которые вы не найдете нигде более. Кроме того, мы предлагаем персонализированное мыло, которое станет идеальным подарком для ваших близких и друзей.</p>
                <p>Мы стремимся обеспечить нашим клиентам лучшее обслуживание и продукты высокого качества. Мы уделяем особое внимание деталям и стараемся удовлетворить все ваши потребности и пожелания. </p>
                <p>Мы надеемся, что наше мыло подарит вам не только удовольствие от использования, но и сделает вашу кожу здоровой и ухоженной. Спасибо за то, что выбрали наш магазин!</p>
            </div>
            <Row>
                <Col>
                    <Image src="https://localhost:7287/about1.png" rounded className="large-image" />
                </Col>
            </Row>
            <Row>
                <Col xs={6} md={4} className="image-container">
                    <Image src="https://localhost:7287/about2.png" rounded className="small-image" />
                    <Image src="https://localhost:7287/about3.png" rounded className="small-image" />
                </Col>
            </Row>
            <Row>
                <Col xs={6} md={4} className="image-container">
                    <Image src="https://localhost:7287/about4.png" rounded className="small-image" />
                    <Image src="https://localhost:7287/about5.png" rounded className="small-image" />
                </Col>
            </Row>
        </Container>
    );
}