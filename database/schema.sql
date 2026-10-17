--
-- PostgreSQL database dump
--

\restrict KjcxZAXaSKmduDMaEPzC2LAKM8ThhltycrRIs3e5SgmiPdgfo8rwibbPpC2B1HR

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: professor; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.professor (
    id_professor integer CONSTRAINT "PROFESSOR_ID_PROFESSOR_not_null" NOT NULL,
    n_user integer CONSTRAINT "PROFESSOR_N_USER_not_null" NOT NULL,
    n_course integer,
    n_subject integer
);


ALTER TABLE public.professor OWNER TO postgres;

--
-- Name: PROFESSOR_ID_PROFESSOR_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.professor ALTER COLUMN id_professor ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."PROFESSOR_ID_PROFESSOR_seq"
    START WITH 500
    INCREMENT BY 1
    MINVALUE 500
    MAXVALUE 1000
    CACHE 1
);


--
-- Name: student; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.student (
    id_student integer CONSTRAINT "STUDENT_ID_STUDENT_not_null" NOT NULL,
    n_user integer,
    n_student_enrollment integer,
    n_course integer
);


ALTER TABLE public.student OWNER TO postgres;

--
-- Name: STUDENT_ID_STUDENT_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.student ALTER COLUMN id_student ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."STUDENT_ID_STUDENT_seq"
    START WITH 800
    INCREMENT BY 1
    MINVALUE 800
    MAXVALUE 1000
    CACHE 1
);


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id_user integer CONSTRAINT "USER_ID_USER_not_null" NOT NULL,
    s_name character varying(30) CONSTRAINT "USER_S_NAME_not_null" NOT NULL,
    b_adult boolean CONSTRAINT "USER_B_ADULT_not_null" NOT NULL,
    n_document integer CONSTRAINT "USER_N_DOCUMENT_not_null" NOT NULL,
    s_password character varying(50),
    s_email character varying(50),
    d_dob date
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: USER_ID_USER_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.users ALTER COLUMN id_user ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."USER_ID_USER_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 500
    CACHE 1
    CYCLE
);


--
-- Name: course; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.course (
    id_course integer CONSTRAINT course_id_couse_not_null NOT NULL,
    n_course integer NOT NULL
);


ALTER TABLE public.course OWNER TO postgres;

--
-- Name: course_id_couse_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.course ALTER COLUMN id_course ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.course_id_couse_seq
    START WITH 17000
    INCREMENT BY 1
    MINVALUE 17000
    MAXVALUE 18000
    CACHE 1
);


--
-- Name: student_subject_notes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.student_subject_notes (
    id_note integer NOT NULL,
    n_student integer NOT NULL,
    n_subject integer NOT NULL,
    n_note integer NOT NULL,
    d_datenote date
);


ALTER TABLE public.student_subject_notes OWNER TO postgres;

--
-- Name: student_subject_notes_id_note_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.student_subject_notes ALTER COLUMN id_note ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.student_subject_notes_id_note_seq
    START WITH 1200
    INCREMENT BY 1
    MINVALUE 1200
    MAXVALUE 1500
    CACHE 1
);


--
-- Name: subject; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subject (
    id_subject integer NOT NULL,
    s_name character varying(30) NOT NULL
);


ALTER TABLE public.subject OWNER TO postgres;

--
-- Name: subject_id_subject_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.subject ALTER COLUMN id_subject ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.subject_id_subject_seq
    START WITH 13000
    INCREMENT BY 1
    MINVALUE 13000
    MAXVALUE 13500
    CACHE 1
);


--
-- Name: professor PROFESSOR_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professor
    ADD CONSTRAINT "PROFESSOR_pkey" PRIMARY KEY (id_professor);


--
-- Name: student STUDENT_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student
    ADD CONSTRAINT "STUDENT_pkey" PRIMARY KEY (id_student);


--
-- Name: users USER_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT "USER_pkey" PRIMARY KEY (id_user);


--
-- Name: course course_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.course
    ADD CONSTRAINT course_pkey PRIMARY KEY (id_course);


--
-- Name: student_subject_notes student_subject_notes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student_subject_notes
    ADD CONSTRAINT student_subject_notes_pkey PRIMARY KEY (id_note);


--
-- Name: subject subject_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subject
    ADD CONSTRAINT subject_pkey PRIMARY KEY (id_subject);


--
-- Name: professor PROFESSOR_N_USER_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professor
    ADD CONSTRAINT "PROFESSOR_N_USER_fkey" FOREIGN KEY (n_user) REFERENCES public.users(id_user) MATCH FULL;


--
-- Name: student STUDENT_N_USER_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student
    ADD CONSTRAINT "STUDENT_N_USER_fkey" FOREIGN KEY (n_user) REFERENCES public.users(id_user);


--
-- Name: student_subject_notes student; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student_subject_notes
    ADD CONSTRAINT student FOREIGN KEY (n_student) REFERENCES public.student(id_student);


--
-- PostgreSQL database dump complete
--

\unrestrict KjcxZAXaSKmduDMaEPzC2LAKM8ThhltycrRIs3e5SgmiPdgfo8rwibbPpC2B1HR

