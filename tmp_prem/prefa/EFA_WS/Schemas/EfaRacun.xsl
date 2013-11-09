<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/RacunReportDts/EfaRacuniIzpisGlavaView">
    <html>
      <head>

        <style type="text/css">

          table {
            width: 90%;
            table-layout: fixed;
            margin-left: 1cm;
            margin-right: 1cm;
          }

          div {
            width: 90%;
            margin-left: 1cm;
            margin-right: 1cm;
          }

          table#center {
            font-weight: bold;
          }

          table#bottom {
            font-weight: bold;
            border-top: black solid 2px;
            border-bottom: black solid 2px;
          }

          .border {
            border: black solid 1px;
          }

          table.center {
            text-align: center;
          }

          td.right {
            text-align: right;
          }

          td.left {
            text-align: left;
          }

          td.center {
            width: 10%;
          }

        </style>

        <title>
          <xsl:value-of select="DokumentOpisInStevilka"/>
        </title>
      </head>


      <body>

        <!-- podatki o komitentu in posti -->
        <table>
          <tr>
            <td>
              <xsl:value-of select="KomitentPlacnikNaziv"/>
              <br />
              <xsl:value-of select="KomitentPlacnikNaslov"/>
              <br />
              <xsl:value-of select="KomitentPlacnikPostaId"/>
              <xsl:text> </xsl:text>
              <xsl:value-of select="KomitentPlacnikPostaNaziv"/>
            </td>
            <td class="center"></td>
            <td>
              <xsl:copy-of select="EfaRacuniIzpisTekstiObjektovView/Besedilo[../ImeObjekta = 'txtGlavaPodatki']"/>
            </td>
          </tr>

          <tr></tr>

          <tr>
            <td class="right">
              <xsl:value-of select="KomitentPlacnikIdDdv"/>
            </td>
            <td class="center"></td>
            <td>
              <xsl:value-of select="PeIzstaviRacunNaziv"/>
              <br />
              <xsl:value-of select="PeIzstaviRacunNaslov"/>
              <br />
              <xsl:value-of select="PeIzstaviRacunKraj"/>
            </td>
          </tr>

          <tr></tr>

          <tr>
            <td>Naslov za pošiljanje</td>
            <td class="center"></td>
            <td>
              Telefon: <xsl:value-of select="FakturistTel" />
              <br />
              Telefax: <xsl:value-of select="FakturistFax" />
            </td>
          </tr>

          <tr></tr>

          <tr>
            <td>
              <xsl:value-of select="DobavniNaslovNaziv"/>
              <br />
              <xsl:value-of select="DobavniNaslovNaslov"/>
              <br />
              <xsl:value-of select="DobavniNaslovKraj"/>
              <br />
              <xsl:value-of select="DobavniNaslovPostaId"/>
              <xsl:text> </xsl:text>
              <xsl:value-of select="DobavniNaslovPostaNaziv"/>
            </td>
            <td class="center"></td>
            <td>
              <b>
                <xsl:value-of select="DokumentOpisInStevilka"/>
              </b>
              <br />
              Naša številka: <xsl:value-of select="NasaStevilka"/>
              <br />
              <xsl:value-of select="DokumentKrajDatum"/>
              <br />
              Valuta: <xsl:value-of select="ValutaOznaka"/>
              <br />
              Na osnovi pog./naročila št. <xsl:value-of select="PogodbaSt"/>
              dne: <xsl:value-of select="PogodbaDatum"/>
              <br />
              Datum opravljenega prometa: <xsl:value-of select="DatumOpravljenihStoritev"/>


            </td>
          </tr>
        </table>

        <br />
        <br />

        <!-- teksti glava -->
        <table class="center">
          <tr>
            <td class="left">
              <xsl:apply-templates select="EfaRacunIzpisTekstiGlava" />
            </td>
          </tr>
        </table>

        <br />
        <br />

        <!-- naslovna vrstica -->
        <table id="center" class="center border">
          <tr>
            <td>Blago-storitev</td>
            <td>Enota</td>
            <td>Količina</td>
            <td>Cena</td>
            <td colspan="2">Popust</td>
            <td>DDV</td>
            <td>Šifra</td>
            <td>Vrednost</td>
          </tr>
          <tr>
            <td />
            <td>Mere</td>
            <td />
            <td>v EUR</td>
            <td>%</td>
            <td>EUR</td>
            <td>v %</td>
            <td>izjave</td>
            <td>v EUR</td>
          </tr>
        </table>

        <!-- seznam blaga... -->
        <table class="center">
          <xsl:apply-templates select="EfaRacuniIzpisPostavkeView" />
        </table>

        <!-- izjave o davku -->
        <table class="center border">
          <tr>
            <td align="left" width="30%">Izjave o davku:</td>
          </tr>
          <xsl:for-each select="EfaRacuniIzpisIzjaveView">
            <tr>
              <td>
                <xsl:value-of select="IzjavaId"/>
              </td>
              <xsl:text> </xsl:text>
              <td align="left">
                <xsl:value-of select="IzjavaOpis"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <br />
        <br />

        <!-- davek view + vsota view -->
        <table>
          <tr>
            <td style="vertical-align: top;">
              <xsl:apply-templates select="EfaRacuniIzpisDavekView" />
            </td>
            <td class="center"></td>
            <td>
              <xsl:apply-templates select="EfaRacuniIzpisVsotaView" />
            </td>
          </tr>
        </table>

        <br />
        <br />
        <br />
        <br />

        <div>
          Račun: PROSIMO, DA SE PRI PRAČILU FAKTURE SKLICUJETE NA ŠT.: <xsl:value-of select="Sklic"/> <br />
          Od dneva zapadlosti terjatve zaračunamo zakonsko določene zamudne obresti.
        </div>

        <br />
        <br />

        <!-- teksti noga -->
        <table class="center">
          <tr>
            <td class="left">
              <xsl:apply-templates select="EfaRacunIzpisTekstiNoga" />
            </td>
          </tr>
        </table>

        <br />
        <br />

        <div>
          Reklamacije sprejemamo v roku 8 dni od dneva izstavitve računa. Pri reklamaciji navedite
          številko in datum računa. V primeru neupravičene reklamacije zaračunavamo zakonsko
          določene zamudne obresti.
        </div>


        <br />
        <br />

        <div>
          Fakturiral: <xsl:value-of select="Fakturiral"/> <br />
          Vodja: <xsl:value-of select="Vodja"/>
        </div>

        <br />
        <br />
        <br />
        <br />

        <div>
          <xsl:copy-of select="EfaRacuniIzpisTekstiObjektovView/Besedilo[../ImeObjekta = 'txtFooter']" />
        </div>


      </body>

    </html>
  </xsl:template>

  <!-- davek view -->
  <xsl:template match="EfaRacuniIzpisDavekView">
    DDV: <xsl:value-of select="DavekProcent"/>%, OSNOVA: <xsl:value-of select="Osnova"/>, ZNESEK: <xsl:value-of select="Vrednost"/>
    <br />
  </xsl:template>

  <!-- teksti glava & noga -->
  <xsl:template match="EfaRacunIzpisTekstiGlava | EfaRacunIzpisTekstiNoga">
    <xsl:for-each select="Tekst">
      <xsl:value-of select="."/>
      <br />
    </xsl:for-each>
  </xsl:template>

  <!-- vsota view -->
  <xsl:template match="EfaRacuniIzpisVsotaView">
    <table>
      <tr>
        <td class="left">VREDNOST BLAGA/STORITEV BREZ P.:</td>
        <td class="right">
          <xsl:value-of select="SkupajVrednostBrezPopusta"/>
        </td>
      </tr>
      <tr>
        <td class="left">SKUPAJ POPUST.:</td>
        <td class="right">
          <xsl:value-of select="SkupajPopust"/>
        </td>
      </tr>
      <tr>
        <td class="left">SKUPAJ DAVEK:</td>
        <td class="right">
          <xsl:value-of select="SkupajDavek"/>
        </td>
      </tr>
    </table>
    <table id="bottom">
      <tr>
        <td class="left">SKUPAJ ZA PLAČILO:</td>
        <td class="right">
          <xsl:value-of select="SkupajZaPlacilo"/>
        </td>
      </tr>
    </table>
  </xsl:template>


  <!-- izpis postavke view -->
  <xsl:template match="EfaRacuniIzpisPostavkeView">
    <tr>
      <td>
        <xsl:value-of select="StoritevNaziv"/>
      </td>
      <td>
        <xsl:value-of select="EnotaMereKratica"/>
      </td>
      <td>
        <xsl:value-of select="Kolicina"/>
      </td>
      <td>
        <xsl:value-of select="CenaNeto"/>
      </td>
      <td>
        <xsl:value-of select="PopustProcent"/>
      </td>
      <td>
        <xsl:value-of select="PopustVrednost"/>
      </td>
      <td>
        <xsl:value-of select="DavekProcent"/>
      </td>
      <td>
        <xsl:value-of select="IzjavaId"/>
      </td>
      <td>
        <xsl:value-of select="CenaNetoDejanska"/>
      </td>
    </tr>

    <xsl:variable name="rowId" select="RowId" />

    <!-- vsa stroskovna mesta -->
    <xsl:for-each select="../EfaRacuniIzpisPostavkeStrmView[RowId = $rowId]">
      <tr>
        <td>
          <xsl:value-of select="StrmId"/>
        </td>
        <td>
          <xsl:value-of select="EnotaMereKratica"/>
        </td>
        <td>
          <xsl:value-of select="Kolicina"/>
        </td>
      </tr>
    </xsl:for-each>

    <tr>
      <td colspan="9" align="left">
        <xsl:value-of select="Opomba"/>
      </td>
    </tr>
    <tr>
      <td colspan="9" align="center">
        <hr />
      </td>
    </tr>

  </xsl:template>


</xsl:stylesheet>