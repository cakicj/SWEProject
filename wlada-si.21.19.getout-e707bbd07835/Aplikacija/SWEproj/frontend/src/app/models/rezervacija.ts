export class Rezervacija
{
    public rezervacija: boolean;
    public id: number;
    public idKorisnika: number;
    constructor()
    {
        this.rezervacija = false;
        this.id = 0;
        this.idKorisnika = 0;
    }

    getRezervacija()
    {
        return this.rezervacija;
    }

    rezervisi(id: number)
    {
        this.idKorisnika = id; 
    }
}