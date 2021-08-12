# Bank Vault

## Discription
Du skal lave en Vault eller "bankboks", der g�r det muligt at opbevare personlige dokumenter, passwords og andet p� en sikker m�de s� kun personer med korrekt key kan f� adgang.

Form�let er at tr�ne emnerne Hashing og Encryption.

Man kan v�lge forskellige scenarier, som f.eks.:

- En desktop applikation (Console, WPF eller Winforms), som kr�ver et password for at dekryptere en fil.
- Eller man kan lave en webapp, som kan gemme ens password p� en sikker m�de. Derved kr�ver blot at man skal huske et eneste password.
- Man kan ogs� l�gge v�gten p� Signing af emails og demonstrere en l�sning der b�de signer og krypterer mails i f.eks. Outlook.
- Der er ogs� mulighed for selv at formulere et opl�g inden for rammerne af faget.

## Krav

#### Data som skal gemmes

- [ ] Konto ID
- [ ] Hash
- [ ] Salt
- [ ] Private key
- [ ] Public key
- [ ] AES key
- [ ] IV
- [ ] Konto bel�b (Krypteret)

#### Funktioner

- [ ] Hashing af kode med salt
- [ ] Login
- [ ] Oprettelse af bruger
- [ ] Kryptering
- [ ] Dekryptering

#### Ekstra

- [ ] �ndring af kode med nu salt